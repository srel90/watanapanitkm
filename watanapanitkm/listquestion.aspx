<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listquestion.aspx.cs" Inherits="wattanapanitkm.listquestion" %>

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
                    <h1>Web Board</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Web Board</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Web Board</h3>
								</div>
								<div class="panel-body">
									<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="listquestion.aspx" enctype="multipart/form-data">
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
                    <div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Add Question</h3>
								</div>
								<div class="panel-body">
									<form class="form-horizontal" role="form" id="scriptFormAdd" name="scriptFormAdd" method="post" action="listquestion.aspx" enctype="multipart/form-data">
										<input type="hidden" id="txt-modeinsert" name="mode" value="insert">
										<div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Full Name</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-fullName" name="fullName" placeholder="Name" value="<% Response.Write(((System.Data.DataTable)Session["USER"]).Rows[0]["name"].ToString());%>" readonly>
													</div>
												</div>	
                                        <div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Title*</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-title" name="title" placeholder="Title" required validationMessage="Title is required!">
													</div>
												</div>	
                                        <div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Question*</label>
												  <div class="col-sm-3">
													<textarea class="form-control" id="txt-question" name="question" placeholder="Question" required validationMessage="Question is required!" ></textarea>
													</div>
												</div>
                                        <div class="form-group">
												  <div class="col-sm-offset-2 col-sm-10">
													<button type="submit" class="btn btn-primary" id="btn-save">Submit</button>
													<img src="img/loading.gif" id="loading2" style="vertical-align:middle"> </div>
												</div>
												<div class="alert alert-warning" id="alert2"></div>
											</form>                                    
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
							<div class="modal-body" id="answerlist">
								
							</div>

							<div class="modal-footer">
                                <form class="form-horizontal" role="form" id="scriptFormReply" name="scriptFormReply" method="post" action="listquestion.aspx" enctype="multipart/form-data">
                                <input type="hidden" id="Hidden1" name="mode" value="reply">
                                    <input type="hidden" id="txt-qid" name="qid" value="">
										<div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Full Name</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="Text1" name="fullName" placeholder="Name" value="<% Response.Write(((System.Data.DataTable)Session["USER"]).Rows[0]["name"].ToString());%>" readonly>
													</div>
												</div>	
                                        <div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Answer*</label>
												  <div class="col-sm-3">
													<textarea class="form-control" id="Textarea1" name="answer" placeholder="Answer" required validationMessage="Answer is required!" ></textarea>
													</div>
												</div>
                                </form>
							  <button id="btn-ok"  type="button" class="btn btn-default">Ok</button><img src="img/loading.gif" id="loading3" style="vertical-align:middle"> 
							  <button id="btn-cancel" type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
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
		            $('#loading,#loading2,#loading3,#alert,#result,#alert2').hide();
		            var response = ajax("listquestion.aspx", ({ mode: "selectAll" }));
		            if ($.trim(response) != "") {

		                $('#result').html(response).show();
		                $('#paging').html($('#temppage').html());

		            } else {
		                $("#alert").html(response).hide().fadeIn("slow");

		            }
		        }
		        this.eventhandle = function () {
		            $("#scriptForm").submit(function (e) {
		                e.preventDefault();
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

		            });
		            $("#scriptFormAdd").submit(function (e) {
		                e.preventDefault();
		                    var options = {
		                        success: function (response) {
		                            $('#loading2').hide();
		                            if ($.trim(response) != "") {

		                                $('#result').html(response).show();
		                                $('#paging').html($('#temppage').html());
		                                script.clearform();
		                            } else {
		                                $("#alert2").html(response).hide().fadeIn("slow");

		                            }
		                        }
		                    };
		                    $('#loading2').show();
		                    $("#scriptFormAdd").ajaxSubmit(options);

		            });
		            $("#scriptFormReply").submit(function (e) {
		                    e.preventDefault();
		                    var options = {
		                        success: function (response) {
		                            $('#loading3').hide();
		                            if ($.trim(response) == 'true') {
		                                alert("complete transection.");
		                                script.View($('#txt-qid').val());
		                                script.clearform();
		                                
		                            } else {
		                                $("#alert").html('Cannot complete your transection!').hide().fadeIn("slow");
		                            }
		                        }
		                    };
		                    $('#loading3').show();
		                    $("#scriptFormReply").ajaxSubmit(options);

		                });
		            $('#btn-ok').click(function () {
		                $("#scriptFormReply").submit();
		            });
		            $('#btn-search').click(function () {
		                $("#txt-page").val(0);
		                $("#txt-take").val(6);
		            });
		            $('#txt-search').change(function () {
		                $("#txt-page").val(0);
		                $("#txt-take").val(6);
		            });
		            
		        }
		        this.setpage = function (page) {
		            $('#txt-page').val(page);
		            $("#scriptForm").submit();
		            
		        }
		        this.validation = function () {
		            $('#scriptFormReply').kendoValidator().data("kendoValidator");
		            $('#scriptFormAdd').kendoValidator().data("kendoValidator");
		        }//end validator
		        this.View = function (id) {
		            var response = ajax("listquestion.aspx", ({ mode: "searchQuestionByID",ID:id }));
		            if ($.trim(response) != "") {
		                var data = $.parseJSON(response.replace(/\n/g, "\\n"));
		                
		                $('#myModalLabel').html(data.data[0].title);
		                $('#txt-qid').val(data.data[0].Id);
		                var html = '';
		                html = '<p>' + data.data[0].question + '</p>';
		                
		                if (data.data[0].answer != "") {
		                    $.each(data.data, function (index, value) {

		                        html += '<div class="box box-info">';
		                        html += '<div class="box-header">';
		                        html += '<h3 class="box-title"><small>' + value.ansFullName + ' [' + value.ansDateTime + ']</small></h3>';
		                        html += '</div>';
		                        html += '<div class="box-body">';
		                        html += '<p>';
		                        html += value.answer;
		                        html += '</p>';
		                        html += '</div><!-- /.box-body -->';
		                        html += '</div><!-- /.box -->';

		                    });
		                }
		                $('#answerlist').html(html);


		            } else {
		                $("#alert").html(response).hide().fadeIn("slow");

		            }
		            $('#form-view').modal({ keyboard: false, backdrop: 'static' });

		        }
		        this.clearform = function () {

		            HTMLFormElement.prototype.reset.call($('#scriptFormAdd')[0]);
		            HTMLFormElement.prototype.reset.call($('#scriptFormReply')[0]);
		            $("#alert").hide();
		        }//end clearForm
		    }
		</script>
    </body>
</html>
