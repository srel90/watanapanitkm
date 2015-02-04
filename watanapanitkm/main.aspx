<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="wattanapanitkm.main" %>

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
                            <p>Hello,<br><% Response.Write(((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString());%></p>
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
                    <h1>Search</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Search</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Search Solution</h3>
								</div>
								<div class="panel-body">
									<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="main.aspx" enctype="multipart/form-data">
												<input type="hidden" id="txt-mode" name="mode" value="search">
                                        <input type="hidden" id="txt-page" name="page" value="0">
                                        <input type="hidden" id="txt-take" name="take" value="6">
												<div class="form-group">
													  <label for="txt-search" class="col-sm-2 control-label">Search</label>
													  <div class="input-group col-sm-3">				
														<input type="text" class="form-control" id="txt-search" name="search" placeholder="Search" >
														<span class="input-group-btn">
														<button class="btn btn-primary" type="submit" id="btn-search"><i class="fa fa-search"></i></button>
														<img src="img/loading.gif" id="loading" style="vertical-align:middle"></span>
														
														</div>
												</div>	
												<div class="alert alert-warning" id="alert"></div>
											</form>
                                    <div id="result"></div>
                                    
								</div>
                                <div class="panel-footer">
                                    <ul class="pagination pagination-sm  pull-center" style="margin: -10px 0!important" id="paging">

                                    </ul>
                                </div>
							</div>
						</div>
					  </div>
                    <!-- Modal -->
					  <div class="modal fade" id="form-view" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
						<div class="modal-dialog modal-lg">
						  <div class="modal-content">
							<div class="modal-header">
							  <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							  <h4 class="modal-title" id="myModalLabel"></h4>
							</div>
							<div class="modal-body" id="detail">
								
							</div>
							<div class="modal-footer"> 
							  <button id="btn-cancel" type="button" class="btn btn-default" data-dismiss="modal">Close</button>
							</div>
						  </div>
						  <!-- /.modal-content --> 
						</div>
						<!-- /.modal-dialog --> 
					  </div>
					  <!-- /.modal --> 
                </section><!-- /.content -->
            </aside><!-- /.right-side -->
        </div><!-- ./wrapper -->
        <script src="javascripts/jquery-2.1.1.min.js"></script>
		<script src="javascripts/jquery-ui.min.js" type="text/javascript"></script>
		<script src="javascripts/jquery.form.min.js" type="text/javascript"></script>
		<script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script> 
		<script src="javascripts/utility.js" type="text/javascript" ></script> 
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
		            $('#loading,#alert,#result').hide();
		          
		        }
		        this.eventhandle = function () {
		            $('#btn-add').click(function () {
		                if ($('#btn-add').prop('disabled')) return;
		                $("#btn-add,#btn-edit,#btn-delete").prop('disabled', 'disabled');
		                script.clearform();
		                $('#txt-mode').val('insert');
		            });
		            
		            $("#btn-delete").click(function () {
		                if ($("#btn-delete").prop("disabled")) return;
		                var list = $('#data-grid').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record to delete.'); return; }
		                if (confirm('Do you want to delete this record?')) {
		                    if (ajax('link.aspx', ({ mode: 'delete', id: selectedItem.linkID })) == 'true') {
		                        script.clearform();
		                    } else {
		                        alert('Cannot complete your transection!');
		                    }
		                }
		            });
		            $("#scriptForm").submit(function (e) {
		                e.preventDefault();
		                $("#btn-add,#btn-edit,#btn-delete").prop('disabled', '');
		                if (validator.validate()) {
		                    var options = {
		                        success: function (response) {
		                            $('#loading').hide();
		                            if ($.trim(response) != "") {
		                                
		                                $('#result').html(response).show();
		                                $('#paging').html($('#temppage').html());
		                                
		                            } else {
		                                $("#alert").html(response).hide().fadeIn("slow");
		                                
		                            }
		                        }
		                    };
		                    $('#loading').show();
		                    $("#scriptForm").ajaxSubmit(options);
		                    $("#btn-save").prop('disabled', 'disabled');
		                }

		            });
		        }
		        this.setpage = function (page) {
		            $('#txt-page').val(page);
		            $("#scriptForm").submit();
		        }
		        this.getpage = function () {
		            if ($('#txt-page').val() == "1") {
		                return 2;
		            } else {
		                return $('#txt-page').val();
		            }
		        }
		        this.validation = function () {
		            validator = $('#scriptForm').kendoValidator({
		                rules: {
		                    
		                },
		                messages: {
		                   
		                }
		            }).data("kendoValidator");
		        }//end validator
		        this.View = function (jtdaID) {
		            var response = ajax("main.aspx", ({ mode: "searchJTDAByID", jtdaID: jtdaID }));
		            if ($.trim(response) != "") {
		                var data = $.parseJSON(response.replace(/\n/g, "\\n"));
		                $('#myModalLabel').html(data.data[0].title);
		                console.log(data.data[0]);
		                var html = '';
		                html = '<p><b>ชื่องาน:</b>' + data.data[0].title + '</p>';
		                html += '<p><b>ระบบงาน:</b>' + data.data[0].systemName + '</p>';
		                html += '<p><b>ชื่อผู้ปฏิบัติงาน:</b>' + data.data[0].practitioner + '</p>';
		                html += '<p><b>วันที่เสร็จ:</b>' + data.data[0].successDateTime + '</p>';
		                html += '<p><b>Solution:</b>' + data.data[0].solution + '</p>';
		                $('#detail').html(html);


		            } else {
		                $("#alert").html(response).hide().fadeIn("slow");

		            }
		            $('#form-view').modal({ keyboard: false, backdrop: 'static' });

		        }
		        this.clearform = function () {

		            HTMLFormElement.prototype.reset.call($('#scriptForm')[0]);
		            $("#alert").hide();
		            $('#txt-mode').val('search');
		        }//end clearForm
		    }
		</script>
    </body>
</html>
