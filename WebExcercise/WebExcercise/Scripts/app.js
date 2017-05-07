/// <reference path="" />
var myApp = angular.module("myApp", [
  'ngAnimate',
  "ngCookies",
  'ui.router',
  "ngMessages",
  'angular-loading-bar',
  "ui.bootstrap"]);


//angular路由
myApp.config(function ($stateProvider, $urlRouterProvider) {
  $urlRouterProvider.when("", "/login");
  $stateProvider
    .state("login", {
      url: "/login",
      controller: "login_ctr",
      templateUrl: "Scripts/app/views/home/login.html"
    })
});


var chk_global_vars = function ($cookieStore, $rootScope, usr, $location, $http) {
  if (usr != null) {
    $rootScope.logined = true;
    $rootScope.DepartName = usr.UserInfo.DepartName;
    $rootScope.Gender = usr.UserInfo.Gender;
    $rootScope.Id = usr.UserInfo.Id;
    $rootScope.LoginName = usr.UserInfo.LoginName;
    $rootScope.LoginNum = usr.UserInfo.LoginNum;
    $rootScope.LoginPwd = usr.UserInfo.LoginPwd;
    $rootScope.Message = usr.UserInfo.Message;
    $rootScope.MobileNo = usr.UserInfo.MobileNo;
    $rootScope.Name = usr.UserInfo.Name;
    $rootScope.Parent = usr.UserInfo.Parent;
    $rootScope.PictureSrc = usr.UserInfo.PictureSrc;
    $rootScope.Spare = usr.UserInfo.Spare;
    $rootScope.UsrEmail = usr.UserInfo.UsrEmail;
    $rootScope.UsrKey = usr.UserInfo.UsrKey;
    $rootScope.UsrRole = usr.UserInfo.UsrRole;
    $rootScope.VerifyCode = usr.VerifyCode;

    $cookieStore.put('logined', true);
    $cookieStore.put('DepartName', usr.UserInfo.DepartName);
    $cookieStore.put('Gender', usr.UserInfo.Gender);
    $cookieStore.put('Id', usr.UserInfo.Id);
    $cookieStore.put('LoginName', usr.UserInfo.LoginName);
    $cookieStore.put('LoginNum', usr.UserInfo.LoginNum);
    $cookieStore.put('LoginPwd', usr.UserInfo.LoginPwd);
    $cookieStore.put('Message', usr.UserInfo.Message);
    $cookieStore.put('MobileNo', usr.UserInfo.MobileNo);
    $cookieStore.put('Name', usr.UserInfo.Name);
    $cookieStore.put('Parent', usr.UserInfo.Parent);
    $cookieStore.put('PictureSrc', usr.UserInfo.PictureSrc);
    $cookieStore.put('Spare', usr.UserInfo.Spare);
    $cookieStore.put('UsrEmail', usr.UserInfo.UsrEmail);
    $cookieStore.put('UsrKey', usr.UserInfo.UsrKey);
    $cookieStore.put('UsrRole', usr.UserInfo.UsrRole);
    $cookieStore.put('VerifyCode', usr.VerifyCode);


  } else if (typeof $rootScope.logined == "undefined" || $rootScope.logined == null || $rootScope.logined == false) {
    //用户-------------------------------------------------------------------------
    $rootScope.logined = $cookieStore.get('logined');
    $rootScope.DepartName = $cookieStore.get('DepartName');
    $rootScope.Gender = $cookieStore.get('Gender');
    $rootScope.Id = $cookieStore.get('Id');
    $rootScope.LoginName = $cookieStore.get('LoginName');
    $rootScope.LoginNum = $cookieStore.get('LoginNum');
    $rootScope.LoginPwd = $cookieStore.get('LoginPwd');
    $rootScope.Message = $cookieStore.get('Message');
    $rootScope.MobileNo = $cookieStore.get('MobileNo');
    $rootScope.Name = $cookieStore.get('Name');
    $rootScope.Parent = $cookieStore.get('Parent');
    $rootScope.PictureSrc = $cookieStore.get('PictureSrc');
    $rootScope.Spare = $cookieStore.get('Spare');
    $rootScope.UsrEmail = $cookieStore.get('UsrEmail');
    $rootScope.UsrKey = $cookieStore.get('UsrKey');
    $rootScope.UsrRole = $cookieStore.get('UsrRole');
    $rootScope.VerifyCode = $cookieStore.get('VerifyCode');
    //其他---------------------------------------------------------------------------


  }
}
$.goup({
  trigger: 100,
  bottomOffset: 20,
  locationOffset: 30,
  title: '回到顶部',
  titleAsText: false
});



