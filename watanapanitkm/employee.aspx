<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="employee.aspx.cs" Inherits="wattanapanitkm.employee" %>

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
                    <h1>Employee</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Employee</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Employee Management</h3>
								</div>
								<div class="panel-body">
									<div id="data-grid"></div>
									<br>
									<div class="panel panel-default">
										<div class="panel-heading">
										  <h3 class="panel-title">Employee Data</h3>
										</div>
										<div class="panel-body">
											<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="employee.aspx">
												<input type="hidden" id="txt-mode" name="mode" value="insert">
												<input type="hidden" id="txt-id" name="id">
                                                <div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">name</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-name" name="name" placeholder="Name">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-username" class="col-sm-2 control-label">Username</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-username" name="username" placeholder="Username">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-password" class="col-sm-2 control-label">Password</label>
												  <div class="col-sm-3">
													<input type="password" class="form-control" id="txt-password" name="password" placeholder="Password">
													</div>
												</div>
                                             <div class="form-group">
													  <label for="txt-refUserID" class="col-sm-2 control-label">User Reference</label>
													  <div class="input-group col-sm-3">				
														<input type="hidden" id="txt-refUserID" name="refUserID" >
														<input type="text" class="form-control" id="txt-refName" name="refName" placeholder="User Reference" readonly>
														<span class="input-group-btn">
														<button class="btn btn-primary" type="button" id="btn-refUserID"><i class="fa fa-search"></i></button>
														</span>
														
														</div>
												</div>
                                                <div class="form-group">
												  <div class="col-sm-offset-2 col-sm-10">
													<div class="checkbox">
													  <label>
														<input name="status" type="checkbox" id="chk-status" value="1" checked="CHECKED">
														Active </label>
													</div>
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
                    <!-- Modal -->
					  <div class="modal fade" id="form-refuser" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
						<div class="modal-dialog modal-lg">
						  <div class="modal-content">
							<div class="modal-header">
							  <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							  <h4 class="modal-title" id="myModalLabel">Select user reference</h4>
							</div>
							<div class="modal-body">
								<div id="data-grid-refuser"></div>
							</div>
							<div class="modal-footer">
							  <button id="btn-ok"  type="button" class="btn btn-default">Ok</button>
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
		            $('#loading,#alert').hide();
		            $("#data-grid").kendoGrid({
		                dataSource: {
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllEmployee' }),
		                            url: 'employee.aspx'
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
							{ field: "employeeID", title: "Employee ID" },
                            { field: "name", title: "Name" },
                            { field: "username", title: "Username" },
                            { title: "RefUserID | RefUsername", template: '#:refUserID# | #:refusername#' },
                            { field: "status", title: "Status", template: '#=status==1?"Active":"Inactive"#' }
		                ],
		                toolbar: [
							{ template: '<button type="button" class="btn btn-primary" id="btn-add"><span class="glyphicon glyphicon-plus-sign"></span>Add new</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-edit"><span class="glyphicon glyphicon-edit"></span>Edit</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-delete"><span class="glyphicon glyphicon-remove-circle"></span>Delete</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-cancel"><span class="glyphicon glyphicon-repeat"></span>Cancel</button>' }
		                ]
		            });
		            

		        }
		        this.eventhandle = function () {
		            $('#btn-refUserID').click(function () {
		                $("#data-grid-refuser").kendoGrid({
		                    dataSource: {
		                        transport: {
		                            read: {
		                                dataType: "json",
		                                type: "POST",
		                                data: ({ mode: 'selectAllRefUser' }),
		                                url: 'employee.aspx'
		                            }
		                        },
		                        dataType: "json",
		                        autoSync: true,
		                        pageSize: 5,
		                        schema: {
		                            data: "data",
		                            total: "total"
		                        }
		                    },
		                    resizable: true,
		                    filterable: true,
		                    reorderable: true,
		                    sortable: true,
		                    columnMenu: true,
		                    selectable: "row",
		                    pageable: { pageSizes: true, refresh: true },
		                    columns: [
								{ field: "usersID", title: "User ID" },
								{ field: "firstname", title: "Firstname" },
								{ field: "lastname", title: "LastName" },
                                { field: "companyName", title: "Company" },
                                { field: "sectorName", title: "Sector" },
                                { field: "departmentName", title: "Department" },
                                { field: "partName", title: "Part" },
		                    ],
		                    toolbar: [
							{ template: '<div class="input-group col-sm-3"><input type="text" class="form-control" id="txt-search" name="search" placeholder="Search user"><span class="input-group-btn"><button class="btn btn-primary" type="button" id="btn-search" onClick="script.searchUser()"><i class="fa fa-search"></i></button></span></div>' },

		                    ]
		                });
		                $("#data-grid-refuser").data("kendoGrid").dataSource.read();
		                $("#data-grid-refuser").data("kendoGrid").refresh();
		                $('#form-refuser').modal({ keyboard: false, backdrop: 'static' });
		            });
		            $('#btn-ok').click(function () {
		                var list = $('#data-grid-refuser').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record frist.'); return; }
		                $('#form-refuser').modal('hide');
		                $('#txt-refUserID').val(selectedItem.usersID);
		                $('#txt-refName').val(selectedItem.firstname + " " + selectedItem.lastname);
		            });
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

		                $('#txt-id').val(selectedItem.employeeID);
		                $('#txt-name').val(selectedItem.name);
		                $('#txt-username').val(selectedItem.username);
		                $('#txt-refUserID').val(selectedItem.refUserID);
		                $('#txt-refName').val(selectedItem.firstname + " " + selectedItem.lastname);
		                setCHKValue('chk-status', selectedItem.status);
		            });
		            $("#btn-delete").click(function () {
		                if ($("#btn-delete").prop("disabled")) return;
		                var list = $('#data-grid').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record to delete.'); return; }
		                if (confirm('Do you want to delete this record?')) {
		                    if (ajax('employee.aspx', ({ mode: 'delete', id: selectedItem.employeeID })) == 'true') {
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
		                            } else {
		                                $("#alert").html('Cannot complete your transection!').hide().fadeIn("slow");
		                            }
		                        }
		                    };
		                    $('#loading').show();
		                    $("#scriptForm").ajaxSubmit(options);
		                }

		            });
		        }
		        this.searchUser = function () {
		            $("#data-grid-refuser").data("kendoGrid").dataSource.read({ search: $('#txt-search').val(), mode: 'searchRefUser' });
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
		            $('#txt-refUserID').val("");
		        }//end clearForm
		    }
		</script>
    </body>
</html>
