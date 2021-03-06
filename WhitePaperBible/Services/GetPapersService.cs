﻿using System;
using WhitePaperBible.Core.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace WhitePaperBible.Core.Services
{
    public interface IGetPapersService : IBaseService
    {
        Task<List<PaperNode>> Execute();
    }

    public class GetPapersService : BaseService, IGetPapersService
    {
        public async Task<List<PaperNode>> Execute()
        {
            //			Client.OpenURL (Constants.BASE_URI + "papers.json?caller=wpb-iPhone");
            await Client.OpenURL(Constants.BASE_URI + "cmd/home.json?caller=wpb-iPhone");
            return JsonConvert.DeserializeObject<List<PaperNode>>(Client.ResponseText);
        }

        #region implemented abstract members of BaseService

        protected override void HandleSuccess(object sender, EventArgs args)
        {
            // gonna have to do some hard core parsing
            var payload = JsonConvert.DeserializeObject<Payload>(this.Client.ResponseText);
            //			dynamic j = JObject.Parse (this.Client.ResponseText);
            var papers = new List<PaperNode>(payload.papers);
            //var histories = new List<PaperHistoryNode>(payload.popular);


            DispatchSuccess(new GetPapersServiceEventArgs(papers, histories));
        }

        #endregion
    }

    public class GetPapersServiceEventArgs : EventArgs
    {
        public readonly List<PaperNode> Papers;

        public readonly List<PaperHistoryNode> Histories;

        public GetPapersServiceEventArgs(List<PaperNode> papers, List<PaperHistoryNode> histories)
        {
            Papers = papers;
            Histories = histories;
        }
    }

    public class Payload
    {
        //		[DataMember(Name = "papers")]
        public PaperNode[] papers;

        //		[DataMember(Name = "popular")]
        public PaperHistoryNode[] popular;
    }
}

