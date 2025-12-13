<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Controls.Header" %>


<!-- Favicon icon-->
<link rel="icon" href="../assets/images/favicon.png" type="image/x-icon"/>
<link rel="shortcut icon" href="../assets/images/favicon.png" type="image/x-icon"/>
<!-- Google font-->
<link rel="preconnect" href="https://fonts.googleapis.com"/>
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin=""/>
<link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:opsz,wght@6..12,200;6..12,300;6..12,400;6..12,500;6..12,600;6..12,700;6..12,800;6..12,900;6..12,1000&amp;display=swap" rel="stylesheet"/>
<!-- Flag icon css -->
<link rel="stylesheet" href="../assets/css/vendors/flag-icon.css"/>
<!-- iconly-icon-->
<link rel="stylesheet" href="../assets/css/iconly-icon.css"/>
<link rel="stylesheet" href="../assets/css/bulk-style.css"/>
<!-- iconly-icon-->
<%--<link rel="stylesheet" href="../assets/css/themify.css"/>--%>
<!--fontawesome-->
<link rel="stylesheet" href="../assets/css/fontawesome-min.css"/>
<!-- Whether Icon css-->
<link rel="stylesheet" type="text/css" href="../assets/css/vendors/scrollbar.css"/>
<link rel="stylesheet" type="text/css" href="../assets/css/vendors/slick.css"/>
<link rel="stylesheet" type="text/css" href="../assets/css/vendors/slick-theme.css"/>
<!-- App css -->
<link rel="stylesheet" href="../assets/css/style.css"/>
<link id="color" rel="stylesheet" href="../assets/css/color-1.css" media="screen"/>

<link href="../assets/js/toastr/toastr.min.css" rel="stylesheet" />

<!-- jquery-->
<script src="../assets/js/vendors/jquery/jquery.min.js"></script>
 <script src="../assets/js/toastr/toastr.min.js"></script>
  <script src="../assets/js/Custom-Toasts.js"></script>
    


<!-- page-wrapper Start-->
<!-- tap on top starts-->
<div class="tap-top"><i class="iconly-Arrow-Up icli"></i></div>
<!-- tap on tap ends-->
<!-- loader-->
<div class="loader-wrapper">
  <div class="loader"><span></span><span></span><span></span><span></span><span></span></div>
</div>
<div class="page-wrapper compact-wrapper" id="pageWrapper"> 
  <header class="page-header row">
    <div class="logo-wrapper d-flex align-items-center col-auto">
        <h1 class="text-white">LMS</h1>
        <a class="close-btn toggle-sidebar" href="javascript:void(0)">
        <svg class="svg-color">
          <use href="../assets/svg/iconly-sprite.svg#Category"></use>
        </svg></a></div>
    <div class="page-main-header col">
      <div class="header-left">
        
        
      </div>
      <div class="nav-right">
        <ul class="header-right"> 
          
       
         
       
          <li class="profile-nav custom-dropdown">
            <div class="user-wrap">
              <div class="user-img"><img src="../assets/images/profile.png" alt="user"/></div>
              <div class="user-content">
                <h6>Puvan</h6>
                <p class="mb-0">Admin<i class="fa-solid fa-chevron-down"></i></p>
              </div>
            </div>
            <div class="custom-menu overflow-hidden">
              <ul class="profile-body">
                <li class="d-flex"> 
                  <svg class="svg-color">
                    <use href="../assets/svg/iconly-sprite.svg#Profile"></use>
                  </svg><a class="ms-2" href="user-profile.html">Account</a>
                </li>
                <li class="d-flex"> 
                  <svg class="svg-color">
                    <use href="../assets/svg/iconly-sprite.svg#Login"></use>
                  </svg><a class="ms-2" href ="">Log Out</a>
                </li>
              </ul>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </header>
  <!-- Page Body Start-->
  <div class="page-body-wrapper"> 
    <!-- Page sidebar start-->
    <aside class="page-sidebar"> 
      <div class="left-arrow" id="left-arrow"><i data-feather="arrow-left"></i></div>
      <div class="main-sidebar" id="main-sidebar">
          <ul class="sidebar-menu" id="simple-bar">
              <li class="pin-title sidebar-main-title">
                  <div>
                      <h5 class="sidebar-title f-w-700">Pinned</h5>
                  </div>
              </li>
              <li class="sidebar-main-title">
                  <div>
                      <h5 class="lan-1 f-w-700 sidebar-title">General</h5>
                  </div>
              </li>
              <li class="sidebar-list">
                  <i class="fa-solid fa-thumbtack"></i><a class="sidebar-link" href="javascript:void(0)">
                      <svg class="stroke-icon">
                          <use href="../assets/svg/iconly-sprite.svg#Home-dashboard"></use>
                      </svg>
                      <h6>Dashboards</h6><span class="badge">3</span><i class="iconly-Arrow-Right-2 icli"></i>
                  </a>
                  <ul class="sidebar-submenu">
                      <li> <a href="../Admin/UserMaster.aspx">UserMaster</a></li>
                    
                      <li> <a href="dashboard-03.html">Education</a></li>
                  </ul>
              </li>
              <li class="sidebar-list">
                  <i class="fa-solid fa-thumbtack"></i><a class="sidebar-link" href="javascript:void(0)">
                      <svg class="stroke-icon">
                          <use href="../assets/svg/iconly-sprite.svg#Pie"></use>
                      </svg>
                      <h6>Manage Books</h6><i class="iconly-Arrow-Right-2 icli"></i>
                  </a>
                  <ul class="sidebar-submenu">
                      <li> <a href="../Admin/AuthorMaster.aspx">Author Master</a></li>
                      <li> <a href="../Admin/CategoryMaster.aspx">Category Master</a></li>
                      <li><a href="../Admin/BookMaster.aspx">Book Master</a></li>
                      <li><a href="../Admin/BookIssue.aspx">Book Issue</a></li>


                  </ul>
              </li>
              <li class="sidebar-list">
                  <i class="fa-solid fa-thumbtack"></i><a class="sidebar-link" href="javascript:void(0)">
                      <svg class="stroke-icon">
                          <use href="../assets/svg/iconly-sprite.svg#Document"></use>
                      </svg>
                      <h6 class="lan-3">Page layout</h6><i class="iconly-Arrow-Right-2 icli"> </i>
                  </a>
                  <ul class="sidebar-submenu">
                      <li> <a href="box-layout.html">Box Layout</a></li>
                      <li><a href="layout-rtl.html">RTL</a></li>
                      <li> <a href="layout-dark.html">Dark</a></li>
                  </ul>
              </li>
              
             
          </ul>
         
        
              
      </div>
      <div class="right-arrow" id="right-arrow"><i data-feather="arrow-right"></i></div>
    </aside>