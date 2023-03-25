using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Domain.ModLocalInfo
{
    public class BatchAnalysis
    {
        private ModFile.ModFile[] _mfs;
        private LocalInfo[] _localInfosResult;

        public bool Completed { get; private set; } = false;

        public BatchAnalysis(ModFile.ModFile[] mfs)
        {
            _mfs = mfs;
        }

        public void DoBatchAnalysis(ModAnalysisServer analysisServer, CancellationToken cancellationToken)
        {
            if (Completed)
                throw new InvalidOperationException("无法重复执行一个已经完成的合批分析");

            var lis = _mfs
                // 带上取消标记并转换为LocalInfo
                .Select(analysisServer.AnalysisMod)
                // 并行后保证执行顺序，附带取消符号
                .AsParallel().AsOrdered().WithCancellation(cancellationToken)
                // 执行
                .ToArray();

            _localInfosResult = lis;
            // 完成了分析
            Completed = true;
        }

        public void UpdateEntities(LocalInfoRepository liRepo, ModFile.ModFileRepository mfRepo)
        {
            if (!Completed)
                throw new InvalidOperationException("在分析完成前无法更新相关实体");
            if (_mfs == null || _localInfosResult == null)
                return;
            _mfs.Zip(_localInfosResult).ToList().ForEach(pair =>
            {
                (var mf, var li) = pair;
                var savedLi = liRepo.Save(li);
                var newMf = mf with { LocalinfoId = savedLi.Id };
                mfRepo.Update(newMf);
            });
        }

        public static IEnumerable<BatchAnalysis> InBatches(ModFile.ModFile[] mfs, int numPerBatch)
        {
            if (numPerBatch < 1)
                throw new ArgumentException("合批数量不能小于1");

            for (int i = 0; i < mfs.Length; i += numPerBatch)
            {
                yield return new BatchAnalysis(mfs.Skip(i).Take(numPerBatch).ToArray());
            }
        }
    }
}
