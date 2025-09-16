using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace V7
{

    public class LoggingIntercept : ActionFilterAttribute
    {
        private LoggerService.LoggerManager? _logger;
       
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _logger = new LoggerService.LoggerManager();
            try
            {
                string reqEndPoint = actionContext.HttpContext.Request.Path;
                if (actionContext.HttpContext.Request.Method == "POST")
                {
                    //reqBody = actionContext.HttpContext.Request.Body.ReadAsStringAsync().Result;
                    IDictionary<string, object?> argument = actionContext.ActionArguments;
                    if (reqEndPoint == "/api/tools/email")
                    {
                        //V7.Model.EmailParam tmpEmail = (V7.Model.EmailParam)argument["param"]!;
                        //// delete body email
                        //tmpEmail.body = "Body truncated";
                        reqEndPoint += "\nBODY:";// + JsonSerializer.Serialize(tmpEmail);
                    }
                    else
                    {
                        reqEndPoint += "\nBODY:" + JsonSerializer.Serialize(argument.Last().Value);

                    }

                    if (!actionContext.ModelState.IsValid)
                    {
                        string errors = actionContext.ModelState.SelectMany(state => state.Value!.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage + ". "));
                    }
                }
                _logger.LogInfo("REQUEST: " + actionContext.HttpContext.Request.Method + " " + reqEndPoint);

            }
            catch (Exception __ex)
            {
                _logger.LogInfo("LOGING REQUEST ERROR: " + __ex.Message);
            }
            base.OnActionExecuting(actionContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                string responseBody = "";
                // Detect that the result is of type JsonResult
                if (filterContext.Result is ObjectResult)
                {
                    var objResult = filterContext.Result as ObjectResult;
                    //responseBody = JsonSerializer.Serialize(objResult.Value);

                    if (objResult!= null && objResult.Value!=null && objResult.StatusCode == 200)
                    {
                        responseBody = "{";
                        //// Dig into the Data Property
                        int i = 0;
                        foreach (var prop in objResult.Value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            var propName = prop.Name;
                            var propValue = prop.GetValue(objResult.Value, null);


                            if (propName != "values")
                            {
                                if (i > 0)
                                {
                                    responseBody += ",";
                                }
                                responseBody += string.Format("\"{0}\":\"{1}\"", propName, propValue);
                                i++;
                            }
                        }
                        responseBody += "}\n";
                    } else
                    {

                        responseBody = JsonSerializer.Serialize(objResult!.Value);
                    }
                }
                _logger = new LoggerService.LoggerManager();
                _logger.LogInfo("RESPONSE:\n " + responseBody);
            }
            catch (Exception __ex)
            {
                _logger!.LogInfo("LOGING RESPONSE ERROR: " + __ex.Message);
            }

            base.OnResultExecuted(filterContext);
        }
    }

}
