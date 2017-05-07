var test_ctr = myApp.controller("test_ctr", function ($scope, $rootScope, $http, $location, $window, $cookieStore, $interval, $filter) {
  $scope.yzmUrl = 'http://' + window.location.host + '/Helper/VerifyCode.aspx';
  $scope.head_tab1 = true;
  $scope.head_tab2 = false;
  $scope.head_tab3 = false;
  $scope.showByTime = true;
  $scope.showByClient = false;
  $scope.accountInfo = true;
  $scope.newAccount = false;
  $scope.newAccountSuccess = false;
  $scope.head_tab3_step1 = true;
  $scope.head_tab3_step2 = false;
  $scope.head_tab3_step3 = false;
  $scope.pagesize_testbuy = 10;
  $scope.pagesize_usrInfo = 10;
  $scope.ParametTime = '';
  $scope.ParametName = '';
  $scope.modelId = '';
  $scope.selectedDepart = false;
  $scope.excelFilePath = "";
  $scope.asc = true;
  $scope.orderascBool = true;
  $scope.way = '';
  $scope.purchaseDateStr = '';
  $scope.custBrandStr = '';
  //-----------------------------------
  $scope.testState = [
    {State: 0, val: '未购买'},
    {State: 1, val: '已购买'},
    {State: 2, val: '未到货'},
    {State: 3, val: '已到货'}
  ];
  $scope.testDepartName = [
    {Depart: '互联网事业部'},
    {Depart: '商拓部'}
  ];
  //取不到model值解决办法----------------
  $scope.ctrlScope = $scope;
  $scope.ctrlScope.page_testbuy = 1;
  $scope.ctrlScope.page_usrInfo = 1;
  //获取cookie
  chk_global_vars($cookieStore, $rootScope, null, $location, $http);
  //页面切换-----------------------------------------------------------------------------------------------------------
  $scope.changeTab1 = function () {
    $scope.head_tab1 = true;
    $scope.head_tab2 = false;
    $scope.head_tab3 = false;
    $scope.newAccountSuccess = false;
    $scope.GetBuyItemInfosByTime();
  }
  $scope.changeTab2 = function () {
    $scope.head_tab1 = false;
    $scope.head_tab2 = true;
    $scope.head_tab3 = false;
    $scope.changeAccountInfo();
    $scope.GetAllUserInfo();
  }
  $scope.changeTab3 = function () {
    $scope.head_tab1 = false;
    $scope.head_tab2 = false;
    $scope.head_tab3 = true;
    $scope.Step1();
    $scope.formData1 = {};
    $scope.formData2 = {};
    $scope.formData3 = {};
    $scope.formData1.Usrid = $rootScope.Id;
    $scope.formData2.Usrid = $rootScope.Id;
    $scope.formData2.PictureSrc = '无';
    $scope.formData3.Usrid = $rootScope.Id;
    $scope.selectedDepart = false;
  }
  $scope.searchByTime = function () {
    $scope.showByTime = true;
    $scope.showByClient = false;
  }
  $scope.searchByClient = function () {
    $scope.showByTime = false;
    $scope.showByClient = true;
  }
  $scope.changeAccountInfo = function () {
    $scope.accountInfo = true;
    $scope.newAccount = false;
  }
  $scope.changeNewAccount = function () {
    $scope.accountInfo = false;
    $scope.newAccount = true;
    //初始化创建账号表单对象------------------------------------------
    $scope.formData = {};
    $scope.formData.Id = $rootScope.Id;
    $scope.formData.PictureSrc = '无';
    $scope.formDatas = {};
    $scope.formDatas.UserInfo = $scope.formData;
  }
  $scope.Step1 = function () {
    $scope.head_tab3_step1 = true;
    $scope.head_tab3_step2 = false;
    $scope.head_tab3_step3 = false;
  }
  $scope.Step2 = function () {
    $scope.head_tab3_step1 = false;
    $scope.head_tab3_step2 = true;
    $scope.head_tab3_step3 = false;
  }
  $scope.Step3 = function () {
    $scope.head_tab3_step1 = false;
    $scope.head_tab3_step2 = false;
    $scope.head_tab3_step3 = true;
  }
  //获取购买员--------------------------------------------------------------------

  //获取所有购买员------------------------
  $scope.getAllTestName = function (departName) {
    if ($rootScope.UsrRole == 1) {
      var url = "/api/Account/GetAllUser?role=" + 7 + "&departName=" + departName;
      var q = $http.get(url);
      q.success(function (response, status) {
        console.log(response)
        $scope.testName = response;
      });
      q.error(function (response) {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
      //获取部门下的购买员-------------------
    } else if ($rootScope.UsrRole == 2 || $rootScope.UsrRole == 3) {
      var url = "/api/Account/GetAllUser?role=" + 7 + "&departName=" + $rootScope.DepartName;
      var q = $http.get(url);
      q.success(function (response, status) {
        console.log(response)
        $scope.testName = response;
      });
      q.error(function (response) {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
    }
  }

  //获取所有用户信息-----------------------------------------------------------------
  $scope.GetAllUserInfo = function () {
    var url = "/api/Account/GetAllUserInfo?role=" + $rootScope.UsrRole + "&parent=" + $rootScope.Parent + "&page=" + ($scope.ctrlScope.page_usrInfo - 1) + "&pagesize=" + $scope.pagesize_usrInfo;
    var q = $http.get(url);
    q.success(function (response, status) {
      $scope.GetAllUserInfoList = response.Result;
      $scope.count_usrInfo = response.Count;
      console.log(response)
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }
  //创建账号--------------------------------------------------------------------
  //隐藏密码功能
  $scope.hidepwd = function () {
    if ($('#eyebtn').hasClass('fa-eye-slash')) {
      $('.hidepassword').attr('type', 'password');
      $('#eyebtn').removeClass('fa-eye-slash').addClass('fa-eye');
    } else {
      $('.hidepassword').attr('type', 'test');
      $('#eyebtn').removeClass('fa-eye').addClass('fa-eye-slash');
    }
  }
  $scope.Regsit = function () {
    console.log($scope.formDatas);
    if ($scope.formData.UsrRole == 2) {
      $scope.formData.DepartName = '无';
    }
    if (!$scope.formData.LoginName || !$scope.formData.LoginPwd || !$scope.formData.CLoginPwd || !$scope.formData.Gender || !$scope.formData.MobileNo || !$scope.formData.Name || !$scope.formData.Parent || !$scope.formData.UsrEmail || !$scope.formData.UsrRole || !$scope.formData.DepartName) {
      $rootScope.alertError = true;
      $rootScope.error = "您输入的信息不完整";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    } else if ($scope.formData.LoginPwd != $scope.formData.CLoginPwd) {
      $rootScope.alertError = true;
      $rootScope.error = "两次输入的密码不一致";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    } else {
      var urls = "api/Account/Regsit";
      var q = $http.post(
        urls,
        JSON.stringify($scope.formDatas),
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
      q.success(function (response, status) {
        if (response.IsSuccess) {
          $scope.LoginNameShow = response.UserInfo.LoginName;
          console.log(response);
          $scope.accountInfo = false;
          $scope.newAccount = false;
          $scope.newAccountSuccess = true;
          $scope.countDown = 5;
          var timer = $interval(function () {
            $scope.countDown--;
            if ($scope.countDown == 0) {
              $interval.cancel(timer);
              $scope.accountInfo = true;
              $scope.newAccount = false;
              $scope.newAccountSuccess = false;
              $scope.GetAllUserInfo();
            }
          }, 1000);
        } else {
          $rootScope.alertError = true;
          $rootScope.error = response.Message;
          setTimeout(function () {
            $rootScope.alertError = false;
            $('#clickIt').click();
          }, 3000);
        }
      });
      q.error(function () {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
    }
  }

  //按时间获取测购信息------------------------------------------------------------
  $scope.GetBuyItemInfosByTime = function () {
    var url = "/api/TestBuy/GetBuyItemInfosByTime?usrid=" + $rootScope.Id;
    var q = $http.get(url);
    q.success(function (response, status) {
      $scope.GetBuyItemInfosByTimeList = response;
      $scope.searchByTime();
      if (response.length > 0) {
        $scope.GetTestBuyList1(response[0].Name);
      }
      console.log(response)
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  //按客户获取测购信息------------------------------------------------------------
  $scope.GetBuyItemInfosByCust = function () {
    var url = "/api/TestBuy/GetBuyItemInfosByCust?usrid=" + $rootScope.Id;
    var q = $http.get(url);
    q.success(function (response, status) {
      $scope.GetBuyItemInfosByCustList = response;
      $scope.searchByClient();
      if (response.length > 0) {
        $scope.GetTestBuyList2(response[0].Name);
      }
      console.log(response)
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  //获取测购信息详情------------------------------------------------------------
  //按品牌和购买时间查询
  $scope.searchBrandAndpurchaseDate = function () {

  }
  //排序
  $scope.orderasc = function () {
    $scope.asc = !$scope.asc;
    $scope.orderascBool = !$scope.orderascBool;
    $scope.GetTestBuyList();
  }
  //按不同方式排序
  $scope.orderbyWay = function (way) {
    $scope.way = way;
    $scope.GetTestBuyList();
  }
  //按时间查询
  $scope.GetTestBuyList1 = function (time) {
    $cookieStore.put('activeTime', time);
    $rootScope.activeTime = $cookieStore.get('activeTime');
    $scope.ParametTime = time;
    $scope.ParametName = '';
    $scope.GetTestBuyList();
  }
  //按客户查询
  $scope.GetTestBuyList2 = function (name) {
    $cookieStore.put('activeName', name);
    $rootScope.activeName = $cookieStore.get('activeName');
    $scope.ParametName = name;
    $scope.ParametTime = '';
    $scope.GetTestBuyList();
  }
  $scope.GetTestBuyList = function () {
    var url = "/api/TestBuy/GetTestBuyList?usrid=" + $rootScope.Id + "&time=" + $scope.ParametTime + "&custName=" + $scope.ParametName + "&page=" + ($scope.ctrlScope.page_testbuy - 1) + "&pagesize=" + $scope.pagesize_testbuy + "&purchaseDate=" + $scope.purchaseDateStr + "&custBrand=" + $scope.custBrandStr + "&orderby=" + $scope.way + "&orderasc=" + $scope.orderascBool;
    var q = $http.get(url);
    q.success(function (response, status) {
      console.log(response)
      $scope.GetTestBuyInfo = response.Result;
      $scope.count_testbuy = response.Count;
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  //编辑模态框
  $scope.editorModal = function (id) {
    $scope.EditorModelId = id + 'e';
    var url = "/api/TestBuy/GetTestBuyItem?itemId=" + id;
    var q = $http.get(url);
    q.success(function (response, status) {
      $scope.GetTestBuyItemListEditor = response;
      $scope.ctrlScope.selecteState = response.BuyItem.State;
      $scope.ctrlScope.selectedDepartName = response.BuyItem.DepartName;
      $scope.ctrlScope.selectedtestName = response.BuyItem.TestBuyerName;


      //过滤时间格式
      if ($scope.GetTestBuyItemListEditor.BuyInfo != null) {
        if ($scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('PurchaseDate')) {
          $scope.GetTestBuyItemListEditor.BuyInfo.PurchaseDate = $filter('date')($scope.GetTestBuyItemListEditor.BuyInfo.PurchaseDate, 'yyyy-MM-dd');
        }
        if ($scope.GetTestBuyItemListEditor.BuyInfo.ProductionDate != null) {
          $scope.GetTestBuyItemListEditor.BuyInfo.ProductionDate = $filter('date')($scope.GetTestBuyItemListEditor.BuyInfo.ProductionDate, 'yyyy-MM-dd');
        }
      }
      if ($scope.GetTestBuyItemListEditor.VeriInfo != null) {
        if ($scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('VerificationDate')) {
          $scope.GetTestBuyItemListEditor.VeriInfo.VerificationDate = $filter('date')($scope.GetTestBuyItemListEditor.VeriInfo.VerificationDate, 'yyyy-MM-dd');
        }
      }
      $scope.testGuidEditor = response.BuyItem.Id;
      console.log(response);
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }
  //编辑测购信息

  $scope.SaveBuyItemsEditor = function () {
    console.log($scope.GetTestBuyItemListEditor.BuyItem);
    if (!$scope.GetTestBuyItemListEditor.BuyItem.TestBuyer || !$scope.GetTestBuyItemListEditor.BuyItem.CustName || !$scope.GetTestBuyItemListEditor.BuyItem.CustBrand || !$scope.GetTestBuyItemListEditor.BuyItem.SKU || !$scope.GetTestBuyItemListEditor.BuyItem.Shopkeeper || !$scope.GetTestBuyItemListEditor.BuyItem.ShopName || !$scope.GetTestBuyItemListEditor.BuyItem.ShopLink || !$scope.GetTestBuyItemListEditor.BuyItem.ShopCredit || !$scope.GetTestBuyItemListEditor.BuyItem.Registration || !$scope.GetTestBuyItemListEditor.BuyItem.ItemLink || !$scope.GetTestBuyItemListEditor.BuyItem.ItemLink || !$scope.GetTestBuyItemListEditor.BuyItem.AlternativeReason) {
      $rootScope.alertError = true;
      $rootScope.error = "请按要求编辑完整表单";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    } else {
      var urls = "api/TestBuy/SaveBuyItems";
      var q = $http.post(
        urls,
        JSON.stringify($scope.GetTestBuyItemListEditor.BuyItem),
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
      q.success(function (response, status) {
        console.log(response);
        if (response.IsSuccess) {
          $scope.GetBuyItemInfosByTime();
          $rootScope.alertSuccess = true;
          $rootScope.success = "测购表单更改成功";
          setTimeout(function () {
            $rootScope.alertSuccess = false;
            $('#clickIt1').click();
          }, 3000);
        }
      });
      q.error(function () {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
    }
  }

  $scope.SaveBuyInfosEditor = function () {
    console.log($scope.GetTestBuyItemListEditor.BuyInfo);
    if ($scope.GetTestBuyItemListEditor.BuyInfo === null) {
      $scope.GetTestBuyItemListEditor.BuyInfo = {};
    } else {
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('OrderNo')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.OrderNo = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ShipAddress')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ShipAddress = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('Seller')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.Seller = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ContactOfSeller')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ContactOfSeller = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('Logitiscs')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.Logitiscs = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('LogisticsNo')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.LogisticsNo = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('CityOfTheShipper')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.CityOfTheShipper = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ShipperName')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ShipperName = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ShipperAddress')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ShipperAddress = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('Contact')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.Contact = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ProductName')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ProductName = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ProductBatch')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ProductBatch = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('ProductCapacity')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.ProductCapacity = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('Origin')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.Origin = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('TermOfValidity')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.TermOfValidity = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('PictureSrc')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.PictureSrc = '';
      }
      if (!$scope.GetTestBuyItemListEditor.BuyInfo.hasOwnProperty('BuyAddress')) {
        $scope.GetTestBuyItemListEditor.BuyInfo.BuyAddress = '';
      }
    }

    $scope.GetTestBuyItemListEditor.BuyInfo.Itemid = $scope.testGuidEditor;

    var urls = "api/TestBuy/SaveBuyInfos";
    var q = $http.post(
      urls,
      JSON.stringify($scope.GetTestBuyItemListEditor.BuyInfo),
      {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    )
    q.success(function (response, status) {
      console.log(response);
      if (response.IsSuccess) {
        $scope.GetBuyItemInfosByTime();
        $rootScope.alertSuccess = true;
        $rootScope.success = "测购表单更改成功";
        setTimeout(function () {
          $rootScope.alertSuccess = false;
          $('#clickIt1').click();
        }, 3000);
      }
    });
    q.error(function () {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  $scope.SaveVerificationInfosEditor = function () {
    console.log($scope.GetTestBuyItemListEditor.VeriInfo);
    if ($scope.GetTestBuyItemListEditor.VeriInfo === null) {
      $scope.GetTestBuyItemListEditor.VeriInfo = {};
    } else {
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('Identifier')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.Identifier = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('VerificationResult')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.VerificationResult = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('EAPNo')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.EAPNo = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('ReasonIfFake')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.ReasonIfFake = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('VerificationRportIssued')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.VerificationRportIssued = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('Review')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.Review = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('Remark')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.Remark = '';
      }
      if (!$scope.GetTestBuyItemListEditor.VeriInfo.hasOwnProperty('NewClue')) {
        $scope.GetTestBuyItemListEditor.VeriInfo.NewClue = '';
      }
    }

    $scope.GetTestBuyItemListEditor.VeriInfo.Itemid = $scope.testGuidEditor;
    var urls = "api/TestBuy/SaveVerificationInfos";
    var q = $http.post(
      urls,
      JSON.stringify($scope.GetTestBuyItemListEditor.VeriInfo),
      {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    )
    q.success(function (response, status) {
      console.log(response);
      if (response.IsSuccess) {
        $scope.GetBuyItemInfosByTime();
        $rootScope.alertSuccess = true;
        $rootScope.success = "测购表单更改成功";
        setTimeout(function () {
          $rootScope.alertSuccess = false;
          $('#clickIt1').click();
        }, 3000);
      }
    });
    q.error(function () {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  //详情中模态框的值
  $scope.showModal = function (id) {
    $scope.modelId = id;
    var url = "/api/TestBuy/GetTestBuyItem?itemId=" + id;
    var q = $http.get(url);
    q.success(function (response, status) {
      $scope.GetTestBuyItemList = response;
      console.log(response);
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  //选择测购任务指派目标-----------------
  //1.选择部门
  $scope.chooseDepartName = function () {
    delete $scope.formData1.TestBuyer;
    delete $scope.formData1.DepartName;
    $scope.selectedDepart = true;
    //$scope.getAllTestName($scope.ctrlScope.selectedDepartName);
    $scope.getAllTestName($scope.ctrlScope.selectedDepartName.Depart);
    $scope.formData1.DepartName = $scope.ctrlScope.selectedDepartName.Depart;
  }
  //2.选择购买员

  $scope.chooseTestBuyer = function () {
    $scope.formData1.TestBuyer = $scope.ctrlScope.selectedtestName.ID;
  }

  //添加测购单-设置测购状态-------------------------
  $scope.chooseState = function () {
    console.log($scope.ctrlScope.selecteState);
    var url = "/api/TestBuy/SetBuyItemState?id=" + $scope.testGuid + "&state=" + $scope.ctrlScope.selecteState.State;
    var q = $http.get(url);
    q.success(function (response, status) {
      console.log(response)
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  ///编辑测购单-设置测购状态
  $scope.chooseStateEditor = function () {
    var url = "/api/TestBuy/SetBuyItemState?id=" + $scope.testGuidEditor + "&state=" + $scope.ctrlScope.selecteState;
    var q = $http.get(url);
    q.success(function (response, status) {
      console.log(response)
    });
    q.error(function (response) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    });
  }

  //添加测购-------------------------------------------------------------------
  $scope.formData1 = {};
  $scope.formData2 = {};
  $scope.formData3 = {};
  $scope.formData1.Usrid = $rootScope.Id;
  $scope.formData2.Usrid = $rootScope.Id;
  $scope.formData3.Usrid = $rootScope.Id;
  //图片功能暂时没有
  $scope.formData2.PictureSrc = '无';
  //---------------------------------------------------
  $scope.SaveBuyItems = function () {
    console.log($scope.formData1);
    if (!$scope.formData1.TestBuyer || !$scope.formData1.CustName || !$scope.formData1.CustBrand || !$scope.formData1.SKU || !$scope.formData1.Shopkeeper || !$scope.formData1.ShopName || !$scope.formData1.ShopLink || !$scope.formData1.ShopCredit || !$scope.formData1.Registration || !$scope.formData1.ItemLink || !$scope.formData1.ItemLink || !$scope.formData1.AlternativeReason) {
      $rootScope.alertError = true;
      $rootScope.error = "请按要求填写完整表单";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    } else {
      var urls = "api/TestBuy/SaveBuyItems";
      var q = $http.post(
        urls,
        JSON.stringify($scope.formData1),
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
      q.success(function (response, status) {
        console.log(response);
        if (response.IsSuccess) {
          $scope.testGuid = response.Message;
          $scope.formData1.Id = response.Message;
          $scope.formData2.Itemid = response.Message;
          $scope.formData3.Itemid = response.Message;
          $scope.Step2();
        }
      });
      q.error(function () {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
    }
  }
  //--------------------------------------------------------------------------
  $scope.uploadImg = function () {
    $('#uploadImg').click();
  }
  $scope.SaveBuyInfos = function () {
    console.log($scope.formData2);
    if (!$scope.formData2.OrderNo || !$scope.formData2.ShipAddress || !$scope.formData2.Seller || !$scope.formData2.ContactOfSeller || !$scope.formData2.Logitiscs || !$scope.formData2.LogisticsNo || !$scope.formData2.CityOfTheShipper || !$scope.formData2.ShipperName || !$scope.formData2.ShipperAddress || !$scope.formData2.Contact || !$scope.formData2.ProductName || !$scope.formData2.ProductBatch || !$scope.formData2.ProductCapacity || !$scope.formData2.Origin || !$scope.formData2.TermOfValidity || !$scope.formData2.PictureSrc) {
      $rootScope.alertError = true;
      $rootScope.error = "请按要求填写完整表单";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    } else {
      var urls = "api/TestBuy/SaveBuyInfos";
      var q = $http.post(
        urls,
        JSON.stringify($scope.formData2),
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
      q.success(function (response, status) {
        console.log(response);
        if (response.IsSuccess) {
          $scope.Step3();
        }
      });
      q.error(function () {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
    }
  }
  //--------------------------------------------------------------------------
  $scope.SaveVerificationInfos = function () {
    console.log($scope.formData3);
    if (!$scope.formData3.Identifier || !$scope.formData3.VerificationResult || !$scope.formData3.EAPNo || !$scope.formData3.ReasonIfFake || !$scope.formData3.VerificationRportIssued || !$scope.formData3.Review || !$scope.formData3.Remark) {
      $rootScope.alertError = true;
      $rootScope.error = "请按要求填写完整表单";
      setTimeout(function () {
        $rootScope.alertError = false;
        $('#clickIt').click();
      }, 3000);
    } else {
      var urls = "api/TestBuy/SaveVerificationInfos";
      var q = $http.post(
        urls,
        JSON.stringify($scope.formData3),
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
      q.success(function (response, status) {
        if (response.IsSuccess) {
          $scope.formData1 = {};
          $scope.formData2 = {};
          $scope.formData3 = {};
          $scope.formData1.Usrid = $rootScope.Id;
          $scope.formData2.Usrid = $rootScope.Id;
          $scope.formData3.Usrid = $rootScope.Id;
          $scope.changeTab1();
          $scope.GetBuyItemInfosByTime();
        }
      });
      q.error(function () {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      });
    }
  }


  $scope.UpLoadFile_sel = function () {
    $('#uploadExcal').click();
  }
  //上传需导入表格
  $scope.UpLoadFile = function () {
    //创建FormData对象
    var data = new FormData();
    //为FormData对象添加数据
    $.each($('#uploadExcal')[0].files, function (i, file) {
      data.append('upload_file' + i, file);
    });
    //发送数据
    $.ajax({
      url: 'Export/UpLoadFile',
      type: 'POST',
      data: data,
      cache: false,
      contentType: false,        //不可缺参数
      processData: false,        //不可缺参数
      success: function (data) {
        if (data != null && data != undefined) {
          console.log(data);
          $scope.singleExlUpload = data;
          $scope.BatchImportTestBuy();
        }
      },
      error: function () {
        $rootScope.alertError = true;
        $rootScope.error = "导入数据失败";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);
      }
    });
  };

  //导入excal表
  $scope.BatchImportTestBuy = function () {
    //  $scope.excelupload();
    var excelFilePath = $("#uploadExcal").val();
    if (excelFilePath == undefined || excelFilePath == "") {
      return;
    } else {
      var url = "/api/TestBuy/BatchImportTestBuy?Usrid=" + $rootScope.Id + "&departName=" + $rootScope.DepartName + "&excelFilePath=" + encodeURIComponent($scope.singleExlUpload);
      var p = $http.get(url);
      p.success(function (response, status) {
        console.log('picManage_ctr>BatchImportOffImgs');
        if (response == "导入成功") {
          $("#uploadExcal").val("");
          $rootScope.alertSuccess = true;
          $rootScope.success = "导入成功";
          setTimeout(function () {
            $rootScope.alertSuccess = false;
            $('#clickIt1').click();
          }, 3000);
          $scope.GetBuyItemInfosByTime();
        } else {
          alert("请下载表格模板，按照表格模板的格式填写表格");
        }
      });
      p.error(function (e) {
        $rootScope.alertError = true;
        $rootScope.error = "服务器连接出错";
        setTimeout(function () {
          $rootScope.alertError = false;
          $('#clickIt').click();
        }, 3000);

      });
    }
  }

  //获取excal表

  //导出表格

  $scope.ExportTestBuyInfo = function () {

    var urls = "api/TestBuy/ExportTestBuyInfo?departName=" + $rootScope.DepartName + "&custName=" + $scope.ParametName;
    var p = $http.get(urls);
    p.success(function (response, status) {

    });
    p.error(function (e) {
      $rootScope.alertError = true;
      $rootScope.error = "服务器连接出错";
      setTimeout(function () {
        $rootScope.alertError = false;

        $('#clickIt').click();
      }, 3000);

    });
  }

  //退出-----------------------------------------------------------------------
  $rootScope.off = function () {
    $location.path("/login").replace();
    $rootScope.DepartName = '';
    $rootScope.Gender = '';
    $rootScope.Id = '';
    $rootScope.LoginName = '';
    $rootScope.LoginNum = '';
    $rootScope.LoginPwd = '';
    $rootScope.Message = '';
    $rootScope.MobileNo = '';
    $rootScope.Name = '';
    $rootScope.Parent = '';
    $rootScope.PictureSrc = '';
    $rootScope.Spare = '';
    $rootScope.UsrEmail = '';
    $rootScope.UsrKey = '';
    $rootScope.UsrRole = '';
    $rootScope.VerifyCode = '';
    //------------------------------
    $cookieStore.remove('DepartName');
    $cookieStore.remove('Gender');
    $cookieStore.remove('Id');
    $cookieStore.remove('LoginName');
    $cookieStore.remove('LoginNum');
    $cookieStore.remove('LoginPwd');
    $cookieStore.remove('Message');
    $cookieStore.remove('MobileNo');
    $cookieStore.remove('Name');
    $cookieStore.remove('Parent');
    $cookieStore.remove('PictureSrc');
    $cookieStore.remove('Spare');
    $cookieStore.remove('UsrEmail');
    $cookieStore.remove('UsrKey');
    $cookieStore.remove('UsrRole');
    $cookieStore.remove('VerifyCode');
  }


  //自动加载
  $scope.getAllTestName('');
  $scope.GetBuyItemInfosByTime();
});