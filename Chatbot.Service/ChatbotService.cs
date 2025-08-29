using Chatbot.Data.EF;
using Chatbot.Model.Chatbot;
using Chatbot.Model.Intent;
using Chatbot.Model.KeywordBoost;
using Chatbot.Model.Pattern;
using Chatbot.Model.Response;
using Chatbot.Model.Synonym;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace Chatbot.Service
{
    public interface IChatbotService
    {
        Task<ChatResult> Ask(string message);
    }

    public class ChatbotService : IChatbotService
    {
        private readonly DataContext _context;
        private readonly IIntentService _intentService;
        private readonly IPatternService _patternService;
        private readonly IResponseService _responseService;
        private readonly ISynonymService _synonymService;
        private readonly IKeywordBoostService _keywordBoostService;
        private readonly IDbConnectionService _dbConnectionService;
        private readonly ILogger<ChatbotService> _logger;

        private List<IntentVm> _intents = new();
        private List<PatternVm> _patterns = new();
        private List<ResponseVm> _responses = new();
        private List<SynonymVm> _synonyms = new();
        private List<KeywordBoostVm> _keywordBoosts = new();

        private readonly double _clarifyMargin = 0.05;   // chênh lệch nhỏ => hỏi lại
        private readonly double _lowConfidence = 0.6;   // dưới ngưỡng => fallback
        //private readonly string _sessionId;
        //private int? _lastIntentId; // ngữ cảnh đơn giản: ý

        public ChatbotService(DataContext context
            , IIntentService intentService
            , IPatternService patternService
            , IResponseService responseService
            , ISynonymService synonymService
            , IKeywordBoostService keywordBoostService
            , IDbConnectionService dbConnectionService
            , ILogger<ChatbotService> logger)
        {
            _context = context;
            _intentService = intentService;
            _patternService = patternService;
            _responseService = responseService;
            _synonymService = synonymService;
            _keywordBoostService = keywordBoostService;
            _dbConnectionService = dbConnectionService;
            _logger = logger;

        }

        public async Task<ChatResult> Ask(string message)
        {
            _intents = await _intentService.GetAll();
            _patterns = await _patternService.GetAll();
            _responses = await _responseService.GetAll();
            _synonyms = await _synonymService.GetAll();
            _keywordBoosts = await _keywordBoostService.GetAll();

            var user = TextNormalizer.Normalize(message);
            var expanded = ExpandSynonyms(user);
            // 1) chấm điểm cho từng intent
            var scored = ScoreIntents(expanded);
            var ranked = scored.OrderByDescending(s => s.score)
                               .ThenByDescending(i => i.intent.Priority) // ưu tiên intent có priority cao hơn nếu score bằng
                               .ToList();

            var top = ranked.FirstOrDefault();
            if (top.intent == null)
            {
                return FinalizeResponse(user, null, _intents.FirstOrDefault()?.DefaultResponse ?? "Xin lỗi, mình chưa có câu trả lời.", 0.0, ranked);
            }

            var bestScore = top.score;
            var secondScore = ranked.Count > 1 ? ranked[1].score : 0.0;

            if (bestScore < _lowConfidence)
            {
                // fallback intent theo priority cao nhất có default_response, nếu có
                var fallback = _intents.OrderBy(i => i.Priority).FirstOrDefault();
                var text = fallback?.DefaultResponse ?? "Mình chưa chắc câu hỏi này. Bạn mô tả rõ hơn được không?";
                return FinalizeResponse(user, fallback, text, bestScore, ranked);
            }

            if (bestScore - secondScore < _clarifyMargin)
            {
                // Ưu tiên nhãn có priority cao hơn
                if (ranked[0].intent.Priority > ranked[1].intent.Priority)
                {
                    var res = _responses.Where(r => r.IntentId == top.intent.Id)
                                   .OrderBy(r => r.UsageCount)
                                   .ThenBy(r => r.Id)
                                   .FirstOrDefault();
                    return FinalizeResponse(user, ranked[0].intent, res?.ResponseText ?? ranked[0].intent.DefaultResponse, bestScore, ranked);
                }

                // Nếu priority = nhau -> hỏi phân biệt (disambiguation)
                var options = ranked.Take(2).Select(x => x.intent.Name).ToList();
                var clarify = $"Bạn muốn hỏi về: {string.Join(" / ", options)}?";
                return FinalizeResponse(user, null, clarify, bestScore, ranked);
            }

            // 3) lấy câu trả lời đa dạng
            var resp = _responses.Where(r => r.IntentId == top.intent.Id)
                                  .OrderBy(r => r.UsageCount)
                                  .ThenBy(r => r.Id)
                                  .FirstOrDefault();

            var answer = resp?.ResponseText ?? top.intent.DefaultResponse;
            if (resp != null) await _responseService.IncrementResponseUsage(resp.Id);

            //_lastIntentId = top.intent.Id; // lưu ngữ cảnh tối giản
            return FinalizeResponse(user, top.intent, answer, bestScore, ranked);
        }


        #region Utilities: chuẩn hoá tiếng Việt, so sánh mờ
        public static class TextNormalizer
        {
            public static string Normalize(string input)
            {
                if (string.IsNullOrWhiteSpace(input)) return string.Empty;
                var lower = input.Trim().ToLowerInvariant();
                lower = lower.Replace('đ', 'd');
                // bỏ dấu tiếng Việt (Unicode combining marks)
                var normalized = lower.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder(normalized.Length);
                foreach (var c in normalized)
                {
                    var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (uc != UnicodeCategory.NonSpacingMark)
                        sb.Append(c);
                }
                var noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

                // chuẩn hoá khoảng trắng
                var collapsed = string.Join(" ", noDiacritics.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                return collapsed;
            }

            public static List<string> Tokenize(string text)
            {
                if (string.IsNullOrWhiteSpace(text)) return new List<string>();
                // tách theo khoảng trắng và ký tự không phải chữ/số
                var tokens = new List<string>();
                var cur = new StringBuilder();
                foreach (var ch in text)
                {
                    if (char.IsLetterOrDigit(ch)) cur.Append(ch);
                    else
                    {
                        if (cur.Length > 0) { tokens.Add(cur.ToString()); cur.Clear(); }
                    }
                }
                if (cur.Length > 0) tokens.Add(cur.ToString());
                return tokens;
            }
        }

        public static class Fuzzy
        {
            // Levenshtein distance (độ gần: 0 = giống hệt)
            public static int LevenshteinDistance(string a, string b)
            {
                if (a == b) return 0;
                if (string.IsNullOrEmpty(a)) return b.Length;
                if (string.IsNullOrEmpty(b)) return a.Length;

                var d = new int[a.Length + 1, b.Length + 1];
                for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
                for (int j = 0; j <= b.Length; j++) d[0, j] = j;

                for (int i = 1; i <= a.Length; i++)
                {
                    for (int j = 1; j <= b.Length; j++)
                    {
                        int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost
                        );
                    }
                }
                return d[a.Length, b.Length];
            }

            // Similarity chuẩn hoá 0..1 (1 = giống hệt)
            public static double Similarity(string a, string b)
            {
                var sa = new HashSet<string>(a.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                var sb = new HashSet<string>(b.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                if (sa.Count == 0 && sb.Count == 0) return 1.0;

                var inter = new HashSet<string>(sa);
                inter.IntersectWith(sb);

                // tính theo tỉ lệ số từ khớp / số từ dài hơn
                return (double)inter.Count / Math.Max(sa.Count, sb.Count);
            }

            // Jaccard trên tập token
            public static double JaccardTokens(IEnumerable<string> a, IEnumerable<string> b)
            {
                var sa = new HashSet<string>(a);
                var sb = new HashSet<string>(b);
                if (sa.Count == 0 && sb.Count == 0) return 1.0;
                var inter = new HashSet<string>(sa);
                inter.IntersectWith(sb);
                var union = new HashSet<string>(sa);
                union.UnionWith(sb);
                return (double)inter.Count / Math.Max(1, union.Count);
            }
        }
        #endregion

        #region Chuyển đổi các từ viết tắt về đúng nghĩa
        private string ExpandSynonyms(string text)
        {
            // Thay synonym -> main_term (ví dụ: "ubnd" => "uy ban nhan dan")
            var tokens = TextNormalizer.Tokenize(text);
            if (_synonyms.Count == 0) return text;

            var map = _synonyms
                .GroupBy(s => TextNormalizer.Normalize(s.SynonymText))
                .ToDictionary(g => g.Key, g => g.First().MainTerm);

            for (int i = 0; i < tokens.Count; i++)
            {
                var t = tokens[i];
                var norm = TextNormalizer.Normalize(t);
                if (map.TryGetValue(norm, out var main))
                {
                    tokens[i] = main;
                }
            }
            return string.Join(" ", tokens);
        }
        #endregion


        #region Danh các kết quả phù hợp
        private List<(IntentVm intent, double score)> ScoreIntents(string user)
        {
            var userTokens = TextNormalizer.Tokenize(user);

            // gom patterns theo intent để đối sánh
            var byIntent = _patterns.GroupBy(p => p.IntentId)
                                    .ToDictionary(g => g.Key, g => g.ToList());

            var results = new List<(IntentVm intent, double score)>();
            foreach (var intent in _intents)
            {
                var patterns = byIntent.TryGetValue(intent.Id, out var list) ? list : new List<PatternVm>();

                // tính điểm cao nhất trong các pattern của intent
                double best = 0.0;
                foreach (var p in patterns)
                {
                    var norm = TextNormalizer.Normalize(p.PatternText);
                    norm = ExpandSynonyms(norm);
                    //var simStr = Fuzzy.Similarity(user, norm);                // so trùng tổng thể
                    var simTok = Fuzzy.JaccardTokens(userTokens, TextNormalizer.Tokenize(norm)); // so theo token
                    //var combined = 0.6 * simStr + 0.4 * simTok;               // kết hợp đơn giản
                    var combined = simTok;
                    if (combined > best) best = combined;

                    // tăng mạnh nếu exact match (sau normalize)
                    if (user == norm) best = Math.Max(best, 1.0);
                }

                // boost theo từ khoá quan trọng đơn giản
                best += Boost(intent, userTokens);

                // chặn score trong [0,1.2]
                //best = Math.Max(0, Math.Min(1.2, best));

                results.Add((intent, best));
            }
            return results;
        }
        #endregion

        #region Kiểm tra các từ khóa được boost
        private double Boost(IntentVm intent, List<string> userTokens)
        {
            var set = new HashSet<string>(userTokens, StringComparer.OrdinalIgnoreCase);

            var keyword = _keywordBoosts.Where(x => x.IntentId == intent.Id).ToList();

            var boost = keyword.FirstOrDefault(x => userTokens.Contains(x.Keyword));

            return boost?.Boost ?? 0;
        }
        #endregion

        private ChatResult FinalizeResponse(string user, IntentVm intent, string answer, double conf, List<(IntentVm intent, double score)> ranked)
        {
            //_repo.SaveConversation(new ConversationLog
            //{
            //    SessionId = _sessionId,
            //    UserInput = user,
            //    BotResponse = answer,
            //    IntentId = intent?.Id,
            //    Confidence = (float?)conf,
            //    CreatedAt = DateTime.UtcNow
            //});

            return new ChatResult
            {
                Response = answer,
                MatchedIntent = intent,
                Confidence = conf,
                TopCandidates = ranked
            };
        }
    }
}
