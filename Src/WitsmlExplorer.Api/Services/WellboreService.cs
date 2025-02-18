using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using WitsmlExplorer.Api.Query;
using Witsml.ServiceReference;
using WitsmlExplorer.Api.Models;
using WitsmlExplorer.Api.Models.Measure;

namespace WitsmlExplorer.Api.Services
{
    public interface IWellboreService
    {
        Task<Wellbore> GetWellbore(string wellUid, string wellboreUid);
        Task<IEnumerable<Wellbore>> GetWellbores(string wellUid = null);
    }

    // ReSharper disable once UnusedMember.Global
    public class WellboreService : WitsmlService, IWellboreService
    {
        public WellboreService(IWitsmlClientProvider witsmlClientProvider) : base(witsmlClientProvider) { }

        public async Task<Wellbore> GetWellbore(string wellUid, string wellboreUid)
        {
            var query = WellboreQueries.GetWitsmlWellboreByUid(wellUid, wellboreUid);
            var result = await WitsmlClient.GetFromStoreAsync(query, new OptionsIn(ReturnElements.All));
            var witsmlWellbore = result.Wellbores.FirstOrDefault();
            if (witsmlWellbore == null) return null;

            return new Wellbore
            {
                Uid = witsmlWellbore.Uid,
                Name = witsmlWellbore.Name,
                WellUid = witsmlWellbore.UidWell,
                WellName = witsmlWellbore.NameWell,
                Number = witsmlWellbore.Number,
                SuffixAPI = witsmlWellbore.SuffixAPI,
                NumGovt = witsmlWellbore.NumGovt,
                WellStatus = witsmlWellbore.StatusWellbore,
                WellborePurpose = witsmlWellbore.PurposeWellbore,
                WellboreParentUid = witsmlWellbore.ParentWellbore?.UidRef,
                WellboreParentName = witsmlWellbore.ParentWellbore?.Value,
                WellType = witsmlWellbore.TypeWellbore,
                Shape = witsmlWellbore.Shape,
                DTimeKickoff = string.IsNullOrEmpty(witsmlWellbore.DTimKickoff) ? null : StringHelpers.ToDateTime(witsmlWellbore.DTimKickoff),
                Md = (witsmlWellbore.Md == null) ? null : new LengthMeasure { Uom = witsmlWellbore.Md.Uom, Value = decimal.Parse(witsmlWellbore.Md.Value)},
                Tvd = (witsmlWellbore.Tvd == null) ? null : new LengthMeasure { Uom = witsmlWellbore.Tvd.Uom, Value = decimal.Parse(witsmlWellbore.Tvd.Value) },
                MdKickoff = (witsmlWellbore.MdKickoff == null) ? null : new LengthMeasure { Uom = witsmlWellbore.MdKickoff.Uom, Value = decimal.Parse(witsmlWellbore.MdKickoff.Value) },
                TvdKickoff = (witsmlWellbore.TvdKickoff == null) ? null : new LengthMeasure { Uom = witsmlWellbore.TvdKickoff.Uom, Value = decimal.Parse(witsmlWellbore.TvdKickoff.Value) },
                MdPlanned = (witsmlWellbore.MdPlanned == null) ? null : new LengthMeasure { Uom = witsmlWellbore.MdPlanned.Uom, Value = decimal.Parse(witsmlWellbore.MdPlanned.Value) },
                TvdPlanned = (witsmlWellbore.TvdPlanned == null) ? null : new LengthMeasure { Uom = witsmlWellbore.TvdPlanned.Uom, Value = decimal.Parse(witsmlWellbore.TvdPlanned.Value) },
                MdSubSeaPlanned = (witsmlWellbore.MdSubSeaPlanned == null) ? null : new LengthMeasure { Uom = witsmlWellbore.MdSubSeaPlanned.Uom, Value = decimal.Parse(witsmlWellbore.MdSubSeaPlanned.Value) },
                TvdSubSeaPlanned = (witsmlWellbore.TvdSubSeaPlanned == null) ? null : new LengthMeasure { Uom = witsmlWellbore.TvdSubSeaPlanned.Uom, Value = decimal.Parse(witsmlWellbore.TvdSubSeaPlanned.Value) },
                DayTarget = (witsmlWellbore.Md == null) ? null : new DayMeasure { Uom = witsmlWellbore.DayTarget.Uom, Value = int.Parse(witsmlWellbore.DayTarget.Value) },
                DateTimeCreation = StringHelpers.ToDateTime(witsmlWellbore.CommonData.DTimCreation),
                DateTimeLastChange = StringHelpers.ToDateTime(witsmlWellbore.CommonData.DTimLastChange),
                ItemState = witsmlWellbore.CommonData.ItemState
            };
        }

        public async Task<IEnumerable<Wellbore>> GetWellbores(string wellUid = null)
        {
            var start = DateTime.Now;
            var query = string.IsNullOrEmpty(wellUid) ? WellboreQueries.GetAllWitsmlWellbores() : WellboreQueries.GetWitsmlWellboreByWell(wellUid);

            var result = await WitsmlClient.GetFromStoreAsync(query, new OptionsIn(ReturnElements.Requested));
            var wellbores = result.Wellbores
                .Select(witsmlWellbore =>
                    new Wellbore
                    {
                        Uid = witsmlWellbore.Uid,
                        Name = witsmlWellbore.Name,
                        WellUid = witsmlWellbore.UidWell,
                        WellName = witsmlWellbore.NameWell,
                        WellStatus = witsmlWellbore.StatusWellbore,
                        WellType = witsmlWellbore.TypeWellbore,
                        IsActive = StringHelpers.ToBooleanSafe(witsmlWellbore.IsActive),
                        DateTimeLastChange = StringHelpers.ToDateTime(witsmlWellbore.CommonData.DTimLastChange)
                    })
                .OrderBy(wellbore => wellbore.Name).ToList();
            var elapsed = DateTime.Now.Subtract(start).Milliseconds / 1000.0;
            Log.Debug("Fetched {Count} wellbores in {Elapsed} seconds", wellbores.Count, elapsed);
            return wellbores;
        }
    }
}
