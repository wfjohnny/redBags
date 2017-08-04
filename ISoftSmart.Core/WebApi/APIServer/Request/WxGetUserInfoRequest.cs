using ISoftSmart.Core.WebApi.APIServer.Response;
using ISoftSmart.Core.WebApi.Util;
using ISoftSmart.Model.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.WebApi.APIServer.Request
{
    public class WxGetUserInfoRequest : APIRequestBase<ChatResponse<bool>>
    {
        public WXUserInfo Chat { get; set; }
        private string _ApiWXUrl=string.Empty;
        public override string ApiRequestSample
        {
            get { return "%BASE_REST_URL%"; }
        }
        public string ApiWXRequestSample
        {
            get {
                return _ApiWXUrl;
            }
            set {
                _ApiWXUrl = value;
            }
        }

        private string method = "POST";
        public override string RequestMethod
        {
            get
            {
                return method;
            }
            set { method = value; }
        }

        public override void PrepareParam()
        {

        }

        protected override void PrepareParam(IDictionary<string, object> paramters)
        {

        }

        public override string GetParamJson()
        {
            return JsonConvert.SerializeObject(Chat, CommonUtil.GetJsonConverters());
        }
    }
}

