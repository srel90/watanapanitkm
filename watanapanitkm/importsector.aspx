<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="importsector.aspx.cs" Inherits="wattanapanitkm.importsector" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Wattapanit Knowledge Management</title>
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport'>
    <meta name="description" content="srel90@gmail.com">
    <meta name="author" content="srel90@gmail.com">
    <link rel="shortcut icon" href="img/favicon.ico">
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <!-- Theme style -->
    <link href="css/style.css" rel="stylesheet" type="text/css" />

    <link href="kendoui/styles/kendo.common-bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="kendoui/styles/kendo.bootstrap.min.css" rel="stylesheet" type="text/css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
          <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
          <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
        <![endif]-->
</head>
<body class="skin-gray">
    <!-- header logo: style can be found in header.less -->
    <header class="header">
        <a href="main.aspx" class="logo">Wattapanit KM</a>
        <!--#include file="navbar-static-top.aspx" -->
    </header>
    <div class="wrapper row-offcanvas row-offcanvas-left">
        <!-- Left side column. contains the logo and sidebar -->
        <aside class="left-side sidebar-offcanvas">
            <!-- sidebar: style can be found in sidebar.less -->
            <section class="sidebar">
                <!-- Sidebar user panel -->
                <div class="user-panel">
                    <div class="pull-left image">
                        <img src="img/avatar.png" class="img-circle" alt="User Image" />
                    </div>
                    <div class="pull-left info">
                        <p>Hello,<br>
                            <% Response.Write(((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString());%></p>
                    </div>
                </div>
                <!--#include file="sidebar-menu.aspx" -->
            </section>
            <!-- /.sidebar -->
        </aside>

        <!-- Right side column. Contains the navbar and content of the page -->
        <aside class="right-side">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Import Sector</h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                    <li class="active">Import Sector</li>
                </ol>
            </section>
            <!-- Main content -->
            <section class="content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title">Sector</h3>
                            </div>
                            <div class="panel-body">
                                <form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="importsector.aspx" enctype="multipart/form-data">
                                    <input type="hidden" id="txt-mode" name="mode" value="import">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Sector excel file</label>
                                        <div class="col-sm-6">
                                            <input id="btnfile" class="form-control" name="file" type="file" /><br>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-offset-2 col-sm-10">
                                            <button type="submit" class="btn btn-primary" id="btn-save">Import</button>
                                            <img src="img/loading.gif" id="loading" style="vertical-align: middle">
                                        </div>
                                    </div>
                                    <div class="alert alert-warning" id="alert"></div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <!-- /.content -->
        </aside>
        <!-- /.right-side -->
    </div>
    <!-- ./wrapper -->
    <script src="javascripts/jquery-2.1.1.min.js"></script>
    <script src="javascripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="javascripts/jquery.form.min.js" type="text/javascript"></script>
    <script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="javascripts/utility.js" type="text/javascript"></script>
    <script src="kendoui/js/kendo.all.min.js" type="text/javascript"></script>
    <!-- App -->
    <script src="javascripts/app.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("body").toggleClass("fixed");
            fix_sidebar();
            script.initial();
            script.validation();
            script.eventhandle();
        });
        var script = new function () {
            var validator = $("#scriptForm");
            this.initial = function () {
                $('#loading,#alert').hide();
            }
            this.eventhandle = function () {
                $("#scriptForm").submit(function (e) {
                    e.preventDefault();
                    if (validator.validate()) {
                        var options = {
                            success: function (response) {
                                $('#loading').hide();
                                if ($.trim(response) == 'true') {
                                    alert("complete transection.");
                                    script.clearform();
                                    $("#btn-save,#btnfile").prop('disabled', '');
                                } else {
                                    $("#alert").html(response).hide().fadeIn("slow");
                                    $("#btn-save,#btnfile").prop('disabled', '');
                                }
                            }
                        };
                        $('#loading').show();
                        $("#scriptForm").ajaxSubmit(options);
                        $("#btn-save,#btnfile").prop('disabled', 'disabled');
                    }

                });
            }
            this.validation = function () {
                validator = $('#scriptForm').kendoValidator({
                    rules: {
                        
                    },
                    messages: {
                       
                    }
                }).data("kendoValidator");
            }//end validator

            this.clearform = function () {
                HTMLFormElement.prototype.reset.call($('#scriptForm')[0]);
                $("#alert").hide();
                $('#txt-mode').val('import');
            }//end clearForm
        }
    </script>
</body>
</html>
