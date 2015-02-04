<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jtda.aspx.cs" Inherits="wattanapanitkm.jtda" %>

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
                    <h1>JTDA Data</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">JTDA Data</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">JTDA Data Management</h3>
								</div>
								<div class="panel-body">
									<div id="data-grid"></div>
									<br>
									<div class="panel panel-default">
										<div class="panel-heading">
										  <h3 class="panel-title">JTDA Data</h3>
										</div>
										<div class="panel-body">
											<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="jtda.aspx" enctype="multipart/form-data">
												<input type="hidden" id="txt-mode" name="mode" value="insert">
												<input type="hidden" id="txt-id" name="id">
												<div class="form-group">
												  <label for="txt-title" class="col-sm-2 control-label">Title*</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-title" name="title" placeholder="Title" required validationMessage="Title is required!">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-title" class="col-sm-2 control-label">Detail*</label>
												  <div class="col-sm-3">
                                                      <textarea class="form-control" id="txt-detail" name="detail" placeholder="Detail" required validationMessage="Detail is required!"></textarea>
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-title" class="col-sm-2 control-label">Solution*</label>
												  <div class="col-sm-3">
                                                      <textarea class="form-control" id="txt-solution" name="solution" placeholder="Detail" required validationMessage="Solution is required!"></textarea>
													</div>
												</div>
												<div class="form-group">
												  <div class="col-sm-offset-2 col-sm-10">
													<button type="submit" class="btn btn-primary" id="btn-save">Save</button>
													<img src="img/loading.gif" id="loading" style="vertical-align:middle"> </div>
												</div>
												<div class="alert alert-warning" id="alert"></div>
											</form>
										</div>
									</div>
								</div>
							</div>
						</div>
					  </div>
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
		            $('#loading,#alert').hide();
		            $("#data-grid").kendoGrid({
		                dataSource: {
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllJTDAByEmployeeID' }),
		                            url: 'jtda.aspx'
		                        }
		                    },
		                    dataType: "json",
		                    autoSync: true,
		                    pageSize: 5,
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    },
		                    serverPaging: true,
		                    serverFiltering: true,
		                    serverSorting: true
		                },
		                /*groupable: true,*/
		                resizable: true,
		                filterable: true,
		                reorderable: true,
		                sortable: true,
		                columnMenu: true,
		                selectable: "row",
		                pageable: { pageSizes: true, refresh: true },
		                columns: [
                            <% if(((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString().Equals("admin")){ %>
							{ field: "jtdaID", title: "ID" },
                            <% } %>
							{ field: "title", title: "Title" },
                            { field: "detail", title: "Detail" },
                            { field: "solution", title: "Solution" },
		                ],
                        
		                toolbar: [
							{ template: '<button type="button" class="btn btn-primary" id="btn-add"><span class="glyphicon glyphicon-plus-sign"></span>Add new</button>&nbsp;' },
                            { template: '<button type="button" class="btn btn-primary" id="btn-edit"><span class="glyphicon glyphicon-edit"></span>Edit</button>&nbsp;' },
                            { template: '<button type="button" class="btn btn-primary" id="btn-delete"><span class="glyphicon glyphicon-remove-circle"></span>Delete</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-cancel"><span class="glyphicon glyphicon-repeat"></span>Cancel</button>' },
                            { template: '&nbsp;<input type="text" id="txt-search" style="height:25px">&nbsp;<button type="button" class="btn btn-primary" id="btn-search" onClick="script.searchJTDA()"><span class="glyphicon glyphicon-search"></span>Search</button>' }
		                ]
                        
		            });
		          
		        }
		        this.eventhandle = function () {
		            $('#btn-add').click(function () {
		                if ($('#btn-add').prop('disabled')) return;
		                $("#btn-add,#btn-edit,#btn-delete").prop('disabled', 'disabled');
		                script.clearform();
		                $('#txt-mode').val('insert');
		            });
		            $('#btn-edit').click(function () {
		                if ($('#btn-edit').prop('disabled')) return;
		                var list = $('#data-grid').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record to edit.'); return; }
		                $("#btn-add,#btn-edit,#btn-delete").prop('disabled', 'disabled');

		                $('#txt-mode').val('update');

		                $('#txt-id').val(selectedItem.jtdaID);
		                $('#txt-title').val(selectedItem.title);
		                $('#txt-detail').val(selectedItem.detail);
		                $('#txt-solution').val(selectedItem.solution);
		            });
		            $("#btn-delete").click(function () {
		                if ($("#btn-delete").prop("disabled")) return;
		                var list = $('#data-grid').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record to delete.'); return; }
		                if (confirm('Do you want to delete this record?')) {
		                    if (ajax('jtda.aspx', ({ mode: 'delete', id: selectedItem.jtdaID })) == 'true') {
		                        script.clearform();
		                    } else {
		                        alert('Cannot complete your transection!');
		                    }
		                }
		            });
		            $("#btn-cancel").click(function () {
		                $("#btn-add,#btn-edit,#btn-delete").prop('disabled', '');
		                script.clearform();
		            });
		            $("#scriptForm").submit(function (e) {
		                e.preventDefault();
		                $("#btn-add,#btn-edit,#btn-delete").prop('disabled', '');
		                if (validator.validate()) {
		                    var options = {
		                        success: function (response) {
		                            $('#loading').hide();
		                            if ($.trim(response) == 'true') {
		                                alert("complete transection.");
		                                script.clearform();
		                                $("#btn-save").prop('disabled', '');
		                            } else {
		                                $("#alert").html(response).hide().fadeIn("slow");
		                                $("#btn-save").prop('disabled', '');
		                            }
		                        }
		                    };
		                    $('#loading').show();
		                    $("#scriptForm").ajaxSubmit(options);
		                    $("#btn-save").prop('disabled', 'disabled');
		                }

		            });
		        }
		        this.searchJTDA = function () {
		            $("#data-grid").data("kendoGrid").dataSource.read({ search: $('#txt-search').val(), mode: 'searchJTDA' });
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
		            $("#data-grid").data("kendoGrid").dataSource.read();
		            $("#data-grid").data("kendoGrid").refresh();
		            $("#data-grid").data("kendoGrid").clearSelection();

		            HTMLFormElement.prototype.reset.call($('#scriptForm')[0]);
		            $("#alert").hide();
		            $('#txt-mode').val('insert');
		        }//end clearForm
		    }
		</script>
    </body>
</html>
