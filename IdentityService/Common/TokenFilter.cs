using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Common
{
    public class TokenFilter : Attribute, IAuthorizationFilter
    {

        /// <summary>
        /// 注入配置信息，注入cacheManager
        /// </summary>
        /// <param name="cacheManager"></param>


        /// <summary>
        /// 验证授权事件
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor action))
            {
                return;
            }

            //如果Controller加了[AllowAnonymous]特性就跳过权限检查
            if (action.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
                return;

            //如果方法上加了[AllowAnonymous]，就跳过权限检查
            if (action.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
                return;

            string authorizationHeaderValue = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authorizationHeaderValue))
            {
                TokenErrorJson(context, "无Token信息");
                return;
            }

            string authorizationHeader = authorizationHeaderValue.Split(" ").Last();




            //var C_UID = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UID");
            //var C_APPUID = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "APPUID");

            //var C_APPTAG = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "APPTAGID");
            //var C_Guid = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Guid");

            //var C_EXP = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "exp");
            //if (C_UID == null || C_APPTAG == null || C_EXP == null || C_APPUID == null || C_Guid == null)
            //{
            //    TokenErrorJson(context, "Token数据异常");
            //    return;
            //}

            //string UID = C_UID.Value;
            //string APPTAG = C_APPTAG.Value;
            //string UnixExp = C_EXP.Value;
            //string APPUID = C_APPUID.Value;
            //string Guid = C_Guid.Value;

            //鉴别TOKEN是否一致
            //string Rediskey = string.Format("Acadsoc.IES.Api:UserToken:{0}-{1}-{2}", UID, APPTAG, Guid);

            //获得现有Token
            //if (_cacheManager.Get<string>(Rediskey) != authorizationHeader)
            //{
            //    TokenErrorJson(context, "Token不匹配");
            //    return;
            //}
         Dictionary<string,object> dic= JsonConvert.DeserializeObject<Dictionary<string,object>>(ValidateJwtToken(authorizationHeader, "secret"));
            //DateTime CheckTime =Convert.ToDateTime(UnixExp);
            //if (CheckTime < DateTime.Now)
            //{
            //    TokenErrorJson(context, "Token已过期");
            //}
        }

        private void TokenErrorJson(AuthorizationFilterContext context, string errorMsg = "签名验证失败!")
        {
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.ContentType = "application/json";
            JsonResult result = new JsonResult(new {msg= errorMsg });//设置返回字段
            context.Result = result;
        }


        private static string ValidateJwtToken(string token, string secret)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm alg = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
                var json = decoder.Decode(token);
                //校验通过，返回解密后的字符串
                return json;
            }
            catch (TokenExpiredException)
            {
                //表示过期
                return "expired";
            }
            catch (SignatureVerificationException)
            {
                //表示验证不通过
                return "invalid";
            }
            catch (Exception)
            {
                return "error";
            }
        }
    }
}
