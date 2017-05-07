using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Data;
using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using System.Text.RegularExpressions;
using cPlusPlusTest.Model;

namespace cPlusPlusTest
{
    public class JiebaController
    {
        #region 自动摘要
        /// <summary>
        /// 核心句序号
        /// </summary>
        public static List<int> keySentenceList = new List<int>();
        /// <summary>
        /// 关键词列表
        /// </summary>
        public static List<string> keywordList = new List<string>();
        /// <summary>
        /// 全文句子列表
        /// </summary>
        public static List<string> AllsentenceList = new List<string>();

        /// <summary>
        /// 自动摘要
        /// </summary>
        /// <param name="categoryName">关键词组名</param>
        /// <param name="time">查询时间</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="source">折线图返回自动摘要时使用</param>
        /// <returns></returns>
        public List<string> GetSummary(string categoryName, string time, string projectId,string source)
        {
            List<string> result = new List<string>();

            //拆分projectId，判断是否为多项目
            var proIds = projectId.Split(';').ToList();

            string text = "";       //自动摘要使用文本
            if (source == null)
            {
                //获取起止时间
                DateTime endTime = new DateTime();
                DateTime.TryParse(time, out endTime);
                DateTime startTime = endTime.AddDays(-1);

                //获取词组ID
                var builderCatId = Builders<IW2S_KeywordCategory>.Filter;
                var filterCatId = builderCatId.Eq(x => x.Name, categoryName);
                if (proIds.Count == 1)
                    filterCatId &= builderCatId.Eq(x => x.ProjectId, new ObjectId(projectId));
                var categoryId = MongoDBHelper.Instance.GetIW2S_KeywordCategorys().Find(filterCatId).Project(x => x._id).FirstOrDefault();

                //获取关键词ID列表
                var builderKey = Builders<IW2S_KeywordGroup>.Filter;
                var filterKey = builderKey.Eq(x => x.CommendCategoryId, categoryId);
                var keywordList = MongoDBHelper.Instance.GetIW2S_KeywordGroups().Find(filterKey).Project(x => x.BaiduCommendId.ToString()).ToList();

                //获取链接标题及摘要
                var builder = Builders<IW2S_level1link>.Filter;
                var filter = builder.In(x => x.SearchkeywordId, keywordList) & builder.Ne(x => x.PublishTime, "");
                var queryDatas = MongoDBHelper.Instance.GetIW2S_level1links().Find(filter).Project(x => new
                {
                    Title = x.Title,
                    Description = x.Description,
                    PublishTime = x.PublishTime
                }).ToList();

                //获取起止时间内数据
                List<TimeLink> LinkInfo = new List<TimeLink>();
                foreach (var gr in queryDatas)
                {
                    TimeLink v = new TimeLink();
                    v.Title = gr.Title;
                    v.Description = gr.Description;
                    DateTime dt = new DateTime();
                    DateTime.TryParse(gr.PublishTime, out dt);
                    v.PublishTime = dt;
                    LinkInfo.Add(v);
                }
                LinkInfo = LinkInfo.Where(x => x.PublishTime > startTime && x.PublishTime <= endTime).ToList();
                foreach (var x in LinkInfo)
                {
                    text += x.Title + "。" + System.Environment.NewLine;
                    text += x.Description + "。" + System.Environment.NewLine;
                }
            }
            else
            {
                text = source;
            }
            
            GetList(text, 3, 1);

            foreach (var i in keySentenceList)
            {
                result.Add(AllsentenceList[i]);
            }
            return result;
        }

        #region 函数集合：提取关键词，抽取核心句
        //初始化结巴分词组件
        JiebaSegmenter segmenter = new JiebaSegmenter();

        /// <summary>
        /// 获取num个核心句
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="num">核心句数</param>
        public void GetList(string text, int num, int type)
        {
            keywordList.Clear();

            //获取核心关键词列表
            switch (type)
            {
                case 1:
                    {
                        TfidfExtractor te = new TfidfExtractor();
                        keywordList = te.ExtractTags(text, num).ToList();
                    }
                    break;
                case 2:
                    {
                        TextRankExtractor te = new TextRankExtractor();
                        keywordList = te.ExtractTags(text, num).ToList();
                    }
                    break;
            }

            AllsentenceList.Clear();
            keySentenceList.Clear();


            //将文章拆为句子列表，并分词
            text = text.Replace(Environment.NewLine.ToString(), " 。");
            text = text.Replace(" ", "");
            AllsentenceList = text.Split('。', '？').Where(x => !string.IsNullOrEmpty(x) && x != "undefined").Select(x => x.Trim()).ToList();
            List<Sentence> temp = new List<Sentence>();
            for (int i = 0; i < AllsentenceList.Count; i++)
            {
                AllsentenceList[i] = AllsentenceList[i] + "。";
                var sentence = segmenter.Cut(AllsentenceList[i]);
                Sentence v = new Sentence();
                v.Sen = string.Join(" ", sentence);
                v.Index = i;
                temp.Add(v);
            }
            GetSentenceList(keywordList, temp);
        }

        /// <summary>
        /// 获取所有关键词对应核心句
        /// </summary>
        /// <param name="keywordList">关键词列表</param>
        /// <param name="sentenceList">全文句子列表</param>
        /// <returns></returns>
        void GetSentenceList(List<string> keywordList, List<Sentence> sentenceList)
        {

            List<Sentence> delList = new List<Sentence>();
            //遍历关键词列表，获取核心句列表
            foreach (var keyword in keywordList)
            {
                //获取包含关键词的核心句
                List<Sentence> tempList = new List<Sentence>();
                foreach (var sentence in sentenceList)
                {
                    if (sentence.Sen.Contains(keyword))
                    {
                        Sentence v = new Sentence();
                        v.Sen = sentence.Sen;
                        v.Index = sentence.Index;
                        tempList.Add(v);
                    }
                }
                //将句子中关键词前面的部分删除
                for (int i = 0; i < tempList.Count; i++)
                {
                    int index = tempList[i].Sen.IndexOf(keyword);
                    tempList[i].Sen = tempList[i].Sen.Substring(index, tempList[i].Sen.Length - index);
                }
                int keySentence = GetKeySentence(keyword, tempList);
                if (keySentence != 10000)
                {
                    keySentenceList.Add(keySentence);
                }
            }

            //对核心句进行排序
            keySentenceList = keySentenceList.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// 获取关键词对应核心句
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="sentenceList">关键词对应的句子词组</param>
        /// <returns></returns>
        int GetKeySentence(string keyword, List<Sentence> sentenceList)
        {
            List<string> nextkeywordList = new List<string>();
            //匹配中文，英文字母和数字及下划线
            Regex reg = new Regex("[\u4e00-\u9fa5_a-zA-Z0-9]+");

            //内容相同的句子数量
            int equalNum = 0;
            for (int i = 0; i < sentenceList.Count; i++)
            {
                if (sentenceList[0].Sen.Equals(sentenceList[i].Sen))
                {
                    equalNum++;
                }
            }
            //如果所有句子都相同，返回第一句
            if (equalNum == sentenceList.Count)
            {
                return ChooseReturnIndex(sentenceList);
            }

            //遍历句子，获取次级关键词列表，并缩短句子
            for (int i = 0; i < sentenceList.Count; i++)
            {
                MatchCollection mc = reg.Matches(sentenceList[i].Sen);
                if (mc.Count > 1)
                {
                    //判断前后关键词是否相同，相同则获取下一个次级关键词
                    int keywordNum = 1;
                    while (mc[keywordNum].Value.Equals(keyword))
                    {
                        keywordNum++;
                    }
                    string nextKeyword = mc[keywordNum].Value;
                    int index = sentenceList[i].Sen.IndexOf(nextKeyword);
                    sentenceList[i].Sen = sentenceList[i].Sen.Substring(index, sentenceList[i].Sen.Length - index);
                    nextkeywordList.Add(nextKeyword);
                }
                else
                {
                    continue;
                }
            }

            if (nextkeywordList.Count <= 0)
            {
                return ChooseReturnIndex(sentenceList);
            }

            //计算次级关键词列表重复项，排除重复数据
            var keytmp = nextkeywordList.GroupBy(x => x).Select(x => new { Word = x.Key, Count = x.Count() }).ToList().OrderByDescending(x => x.Count).ToList();

            int count = 0;
            foreach (var x in keytmp)
            {
                if (x.Count == 1) count++;
                if (count == keytmp.Count)
                {

                    return ChooseReturnIndex(sentenceList);
                }
            }
            List<Sentence> delList = new List<Sentence>();
            for (int i = 0; i < sentenceList.Count; i++)
            {
                Sentence sentence = sentenceList[i];
                string wordtemp = keytmp[0].Word;
                if (wordtemp.Length > sentence.Sen.Length)
                {
                    delList.Add(sentence);
                }
                else
                {
                    string compareWord = sentence.Sen.Substring(0, wordtemp.Length);
                    if (!wordtemp.Equals(compareWord)) delList.Add(sentence);
                }
            }
            foreach (var x in delList)
            {
                sentenceList.Remove(x);
            }
            if (sentenceList.Count <= 0) { return 10000; }
            int keySentence = GetKeySentence(keytmp[0].Word, sentenceList);
            return keySentence;
        }

        /// <summary>
        /// 判断应返回的核心句位置
        /// </summary>
        /// <param name="sentenceList">关键词对应的句子词组</param>
        /// <returns></returns>
        int ChooseReturnIndex(List<Sentence> sentenceList)
        {
            foreach (var sen in sentenceList)
            {
                int count2 = 0;
                foreach (var v in keySentenceList)
                {
                    if (!AllsentenceList[v].Equals(AllsentenceList[sen.Index]))
                    {
                        count2++;
                    }
                }
                if (count2 == keySentenceList.Count) { return sen.Index; }
            }
            return 10000;
        }

        class Sentence
        {
            public string Sen { get; set; }
            public int Index { get; set; }
        }

        class TimeLink
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime PublishTime { get; set; }
        }
        #endregion
        #endregion

        #region 词频图
        //结巴分词初始化
        PosSegmenter posSegementer = new PosSegmenter();

        //百度词频
        public FrequencyResult BaiduFrequency(string usr_id, string projectId, string categoryId)
        {
            //获取词组对应链接的标题与摘要
            var builder = Builders<IW2S_level1link>.Filter;
            List<string> commendIds = new List<string>();

            if (!string.IsNullOrEmpty(categoryId))
            {
                var buildGroup = Builders<IW2S_KeywordGroup>.Filter;
                var filterGroup = buildGroup.In(x => x.CommendCategoryId, GetObjIdListFromStr(categoryId));
                var keywordIdsGroup = MongoDBHelper.Instance.GetIW2S_KeywordGroups().Find(filterGroup).Project(x => x.BaiduCommendId.ToString()).ToList();
                if (keywordIdsGroup.Count > 0)
                {
                    commendIds.AddRange(keywordIdsGroup);
                }
            }
            var filter = builder.In(x => x.SearchkeywordId, commendIds);
            var TaskList = MongoDBHelper.Instance.GetIW2S_level1links().Find(filter).Project(x => string.Format("{0} {1}", x.Title, x.Description)).ToList();
            string text = string.Join(" ", TaskList.ToArray());

            FrequencyResult result = GetFrequency(text);
            return result;
        } 

        /// <summary>
        /// 获取文章内名、动词词频
        /// </summary>
        /// <param name="text">要获取词频的文章</param>
        /// <returns></returns>
        private FrequencyResult GetFrequency(string text)
        {
            //临时数据列表
            List<string> noun = new List<string>();
            List<int> nounCount = new List<int>();
            List<string> verb = new List<string>();
            List<int> verbCount = new List<int>();
            int num = 0;
            int i = 0, j = 0;

            //对数据分词，并统计词频
            var segment = posSegementer.Cut(text).ToList();
            var wordList = segment.GroupBy(x => x.ToString()).Select(x => new { Word = x.Key.ToString(), Count = x.Count() }).OrderByDescending(x => x.Count).ToList();

            //提取名、动词词频
            Regex regN = new Regex("(?<word>[\u4E00-\u9FA5][\u4E00-\u9FA5]+?)/n[a-zA-Z]*");
            Regex regV = new Regex("(?<word>[\u4E00-\u9FA5][\u4E00-\u9FA5]+?)/v[a-zA-Z]*");
            foreach (var wordInfo in wordList)
            {
                if (num == 20) break;
                string word = regN.Match(wordInfo.Word).Groups["word"].Value;
                if (!string.IsNullOrEmpty(word) & i < 10)
                {
                    int nc = wordInfo.Count;
                    noun.Add(word);
                    nounCount.Add(nc);
                    i++;
                    num++;
                }
                else
                {
                    word = regV.Match(wordInfo.Word).Groups["word"].Value;
                    if (!string.IsNullOrEmpty(word) & j < 10)
                    {
                        int nc = wordInfo.Count;
                        verb.Add(word);
                        verbCount.Add(nc);
                        j++;
                        num++;
                    }
                }
            }

            FrequencyResult result = new FrequencyResult();
            result.noun = noun;
            result.nounCount = nounCount;
            result.verb = verb;
            result.verbCount = verbCount;
            return result;
        }

        /// <summary>
        /// 获取IdList
        /// </summary>
        /// <param name="idStr"></param>
        /// <returns></returns>
        private List<ObjectId> GetObjIdListFromStr(string idStr)
        {
            var idArray = idStr.Split(';');
            return idArray.Where(x => !string.IsNullOrEmpty(x) && x != "undefined").Select(x => new ObjectId(x)).ToList();
        }

        public class FrequencyResult
        {
            public List<string> noun { get; set; }
            public List<int> nounCount { get; set; }
            public List<string> verb { get; set; }
            public List<int> verbCount { get; set; }
        }

        //对文本进行切词，以空格隔开
        
        public TextTree WordSegemete(TextTree text)
        {
            var segement = segmenter.Cut(text.TreeValues);
            string str = string.Join(" ", segement);
            str = str.Replace(" 。", ".");
            TextTree t = new TextTree();
            t.TreeValues = str;
            return t;
        }

        public class TextTree
        {
            public string TreeValues { get; set; }
        }
        #endregion

        #region 权重图
        //结巴分词初始化
        TfidfExtractor tfidf = new TfidfExtractor();

        //百度权重图
        public string BaiduExtract(string usr_id, string projectId, string categoryId)
        {
            //获取词组对应链接的标题与摘要
            var builder = Builders<IW2S_level1link>.Filter;
            var filter = builder.Eq(x => x.UsrId, new ObjectId(usr_id)) & builder.Eq(x => x.ProjectId, new ObjectId(projectId)) & !builder.Eq(x => x.DataCleanStatus, (byte)2);

            List<string> commendIds = new List<string>();

            if (!string.IsNullOrEmpty(categoryId))
            {
                var buildGroup = Builders<IW2S_KeywordGroup>.Filter;
                var filterGroup = buildGroup.In(x => x.CommendCategoryId, GetObjIdListFromStr(categoryId)) & buildGroup.Eq(x => x.IsDel, false);
                var keywordIdsGroup = MongoDBHelper.Instance.GetIW2S_KeywordGroups().Find(filterGroup).Project(x => x.BaiduCommendId.ToString()).ToList();
                if (keywordIdsGroup.Count > 0)
                {
                    commendIds.AddRange(keywordIdsGroup);
                }
            }
            if (commendIds.Count > 0)
            {
                filter &= builder.In(x => x.SearchkeywordId, commendIds);
            }

            var TaskList = MongoDBHelper.Instance.GetIW2S_level1links().Find(filter).Project(x => string.Format("{0} {1}", x.Title, x.Description)).ToList();
            string all = string.Join(" ", TaskList.ToArray());

            //获取关键词及其权重并转换成对应格式
            string text = "";
            var wordlist = tfidf.ExtractTagsWithWeight(all, 30).ToList();
            foreach (var x in wordlist)
            {
                //int weight = Convert.ToInt32(x.Weight * 100) * 50 / max;
                //if (weight < 50) weight = weight +10;
                text += "<span data-weight=\"" + Convert.ToInt32(x.Weight * 100) + "\">" + x.Word + "</span>";
            }

            return text;
        }
        #endregion
    }

}