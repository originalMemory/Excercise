﻿var login_ctr = myApp.controller("login_ctr", function ($scope, $rootScope, $http, $location, $window, $cookieStore) {
  $rootScope.alertError = false;
  $rootScope.error = '';
  $scope.loginerror = '';
  $scope.userName = '';
  $scope.password = '';
  $scope.savePwd = true;

  //登陆账号--------------------------------------------------------------------

  //启动enter 事件
  $scope.EnterPress = function () {
    var e = e || window.event;
    if (e.keyCode == 13) {
      $scope.Login();
    }
  }
  $scope.Login = function () {
      var pro = {
          id: "5961d340e0223d0d881a7e91",
          num: 1
      };
      var proList = [];
      proList.push(pro);
      $scope.anaItem = {
          userId: "58845bed1e5318078cb01b1a",
          productList: proList,
      };
      var urls = "http://localhost:2012/api/Pay/InsertOrder";
      var q = $http.post(
        urls,
        JSON.stringify($scope.anaItem),
        {
            headers: {
                'Content-Type': 'application/json'
            }
        }
      )
      q.success(function (response, status) {
          if (response.IsSuccess == true) {
              $scope.addAlert('success', '命名实体添加成功');
          } else {
              $scope.addAlert('danger', response.Message);
          }
      });
      q.error(function (e) {
          $scope.addAlert('danger', "服务器连接出错");
      });
    //if ($scope.userName == '') {
    //  $scope.loginerror = '用户名不能为空';
    //} else if ($scope.password == '') {
    //  $scope.loginerror = '密码不能为空';
    //} else {
    //  var url = "api/Account/Login?usr_name=" + $scope.userName + "&pwd=" + $scope.password;
    //  var q = $http.get(url);
    //  q.success(function (response, status) {
    //      if (response.IsSuccess) {
    //        //记住密码
    //        if ($scope.savePwd) {
    //          $cookieStore.put("uName", $scope.userName);
    //          $cookieStore.put("uPwd", $scope.password);
    //        } else {
    //          $cookieStore.put("uName", '');
    //          $cookieStore.put("uPwd", '');
    //        }
    //        console.log(response);
    //        $scope.userName = '';
    //        $scope.password = '';
    //        chk_global_vars($cookieStore, $rootScope, response, null, $http);

    //        if (response.UsrRole == 4) {
    //            $location.path("/Survey").replace();
    //        } else {
    //            $location.path("/test").replace();
    //        }

            
    //      } else {
    //        $scope.loginerror = response.Message;
    //      }
    //    }
    //  )
    //  q.error(function (err) {
    //    $rootScope.alertError = true;
    //    $rootScope.error = "服务器连接出错";
    //    setTimeout(function () {
    //      $rootScope.alertError = false;
    //      $('#clickIt').click();
    //    }, 3000);
    //  });
    //}
  }

  //1.保存密码___________________________________
  $scope.userName = $cookieStore.get('uName');
  $scope.password = $cookieStore.get('uPwd');

  //记住密码后自动输入密码----------------------------------------------------------
  $scope.autoPwd = function () {
    $scope.loginerror = '';
    if ($cookieStore.get('uName') == $scope.userName) {
      $scope.password = $cookieStore.get('uPwd');
    } else {
      $scope.password = '';
    }
  }
  $scope.clearError = function () {
    $scope.loginerror = '';
  }
});