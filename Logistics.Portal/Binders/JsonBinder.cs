using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Logistics.Portal.Binders {
    public class JsonBinder<T> : DefaultModelBinder {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            //return base.BindModel(controllerContext, bindingContext);
            var request = HttpContext.Current.Request;
            string data = request["data"];
            if (data != null) {
                T model= JsonConvert.DeserializeObject<T>(data);
                return model;
            }
            return null;
        }
    }
}