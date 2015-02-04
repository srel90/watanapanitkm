<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="wattanapanitkm.user" %>

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
                <style>
            .dropdown-header {
                padding: 0!important;
            }

            .dropdown-header > span {
                -webkit-box-sizing: border-box;
                -moz-box-sizing: border-box;
                box-sizing: border-box;
                text-align: left;
                display: inline-block;
                border-style: solid;
                border-width: 0 0 1px 1px;
                padding: .3em .6em;
                width: 74%;
            }

            .dropdown-header > span:first-child {
                width: 105px;
                border-left-width: 0;
            }

            #txt-companyID-list .k-item > span,#txt-sectorID-list .k-item > span,#txt-departmentID-list .k-item > span,#txt-partID-list .k-item > span,#txt-subPartID-list .k-item > span {
                -webkit-box-sizing: border-box;
                -moz-box-sizing: border-box;
                box-sizing: border-box;
                display: inline-block;
                border-style: solid;
                border-width: 0 0 1px 1px;
                vertical-align: top;
                min-height: 35px;
                width: 72%;
                padding: .6em 0 0 .6em;
            }

            #txt-companyID-list .k-item > span:first-child,#txt-sectorID-list .k-item > span:first-child,#txt-departmentID-list .k-item > span:first-child,#txt-partID-list .k-item > span:first-child,#txt-subPartID-list .k-item > span:first-child {
                width: 100px;
                border-left-width: 0;
                padding: .6em 0 0 0;
            }
        </style>
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
                    <h1>Users JTDA</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Users JTDA</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Users JTDA Management</h3>
								</div>
								<div class="panel-body">
									<div id="data-grid"></div>
									<br>
									<div class="panel panel-default">
										<div class="panel-heading">
										  <h3 class="panel-title">Users JTDA Data</h3>
										</div>
										<div class="panel-body">
											<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="user.aspx">
												<input type="hidden" id="txt-mode" name="mode" value="insert">
												<input type="hidden" id="txt-id" name="id">
                                                <div class="form-group">
												  <label for="txt-firstname" class="col-sm-2 control-label">Firstname</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-firstname" name="firstname" placeholder="Firstname">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-lastname" class="col-sm-2 control-label">Lastname</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-lastname" name="lastname" placeholder="Lastname">
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
												  <label for="txt-positionID" class="col-sm-2 control-label">Position</label>
												  <div class="col-sm-3">
													<input type="text"  id="txt-positionID" name="positionID" style="width:400px">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-companyID" class="col-sm-2 control-label">Company</label>
												  <div class="col-sm-3">
													<input type="text"  id="txt-companyID" name="companyID" style="width:400px">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-sectorID" class="col-sm-2 control-label">Sector</label>
												  <div class="col-sm-3">
													<input type="text"  id="txt-sectorID" name="sectorID" style="width:400px">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-departmentID" class="col-sm-2 control-label">Department</label>
												  <div class="col-sm-3">
													<input type="text"  id="txt-departmentID" name="departmentID" style="width:400px">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-partID" class="col-sm-2 control-label">Part</label>
												  <div class="col-sm-3">
													<input type="text"  id="txt-partID" name="partID" style="width:400px">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-subPartID" class="col-sm-2 control-label">Sub-Part</label>
												  <div class="col-sm-3">
													<input type="text"  id="txt-subPartID" name="subPartID" style="width:400px">
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
		                            data: ({ mode: 'selectAllUser' }),
		                            url: 'user.aspx'
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
							{ field: "usersID", title: "ID" },
                            { field: "firstname", title: "Firstname" },
                            { field: "lastname", title: "Lastname" },
                            { field: "username", title: "Username" },
                            { field: "positionName", title: "Position" },
                            { field: "companyName", title: "Company" },
                            { field: "sectorName", title: "Sector" },
                            { field: "departmentName", title: "Department" },
                            { field: "partName", title: "Part" },
                            { field: "subPartName", title: "Sub-Part" },
                            { field: "status", title: "Status", template: '#=status==1?"Active":"Inactive"#' }
		                ],
		                toolbar: [
							{ template: '<button type="button" class="btn btn-primary" id="btn-add"><span class="glyphicon glyphicon-plus-sign"></span>Add new</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-edit"><span class="glyphicon glyphicon-edit"></span>Edit</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-delete"><span class="glyphicon glyphicon-remove-circle"></span>Delete</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-cancel"><span class="glyphicon glyphicon-repeat"></span>Cancel</button>' }
		                ]
		            });
		            $("#txt-positionID").kendoDropDownList({
		                optionLabel: "Select Position",
		                dataTextField: "name",
		                dataValueField: "positionID",
		                headerTemplate: '<div class="dropdown-header">' +
                                            '<span class="k-widget k-header">Position Name</span>' +
                                            '<span class="k-widget k-header">Position Detail</span>' +
                                        '</div>',
		                valueTemplate: '<span class="selected-value">#:data.name==undefined?"":data.name#</span>',
		                template: '<span class="k-state-default">#:data.name==undefined?"":data.name#</span>' +
                                  '<span class="k-state-default">#:data.detail==undefined?"":data.detail#</span>',

		                dataSource: {
		                    type: "json",
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllPosition' }),
		                            url: 'user.aspx'
		                        }
		                    },
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    }
		                }
		            }).data("kendoDropDownList");
		            $("#txt-companyID").kendoDropDownList({
		                optionLabel: "Select Company",
		                dataTextField: "name",
		                dataValueField: "companyID",
		                headerTemplate: '<div class="dropdown-header">' +
                                            '<span class="k-widget k-header">Company Code</span>' +
                                            '<span class="k-widget k-header">Company Name</span>' +
                                        '</div>',
		                valueTemplate: '<span class="selected-value">#:data.code==undefined?"":data.code# #:data.name#</span>',
		                template: '<span class="k-state-default">#:data.code==undefined?"":data.code#</span>' +
                                  '<span class="k-state-default">#:data.name#</span>',

		                dataSource: {
		                    type: "json",
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllCompany' }),
		                            url: 'user.aspx'
		                        }
		                    },
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    }
		                },
		                change: function () {
		                    var course = $("#txt-sectorID").data("kendoDropDownList");
		                    course.dataSource.filter({
		                        field: "companyID",
		                        value: this.value(),
		                        operator: "eq"
		                    });
		                    course.value(0);
		                }
		            }).data("kendoDropDownList");
		            $("#txt-sectorID").kendoDropDownList({
		                optionLabel: "Select Sector",
		                dataTextField: "name",
		                dataValueField: "sectorID",
		                headerTemplate: '<div class="dropdown-header">' +
                                            '<span class="k-widget k-header">Sector Code</span>' +
                                            '<span class="k-widget k-header">Sector Name</span>' +
                                        '</div>',
		                valueTemplate: '<span class="selected-value">#:data.code==undefined?"":data.code# #:data.name#</span>',
		                template: '<span class="k-state-default">#:data.code==undefined?"":data.code#</span>' +
                                  '<span class="k-state-default">#:data.name#</span>',
		                dataSource: {
		                    type: "json",
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllSector' }),
		                            url: 'user.aspx'
		                        }
		                    },
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    }
		                },
		                change: function () {
		                    var course = $("#txt-departmentID").data("kendoDropDownList");
		                    course.dataSource.filter({
		                        field: "sectorID",
		                        value: this.value(),
		                        operator: "eq"
		                    });
		                    course.value(0);
		                }
		            }).data("kendoDropDownList");
		            $("#txt-departmentID").kendoDropDownList({
		                optionLabel: "Select Department",
		                dataTextField: "name",
		                dataValueField: "departmentID",
		                headerTemplate: '<div class="dropdown-header">' +
                                            '<span class="k-widget k-header">Department Code</span>' +
                                            '<span class="k-widget k-header">Department Name</span>' +
                                        '</div>',
		                valueTemplate: '<span class="selected-value">#:data.code==undefined?"":data.code# #:data.name#</span>',
		                template: '<span class="k-state-default">#:data.code==undefined?"":data.code#</span>' +
                                  '<span class="k-state-default">#:data.name#</span>', dataSource: {
		                    type: "json",
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllDepartment' }),
		                            url: 'user.aspx'
		                        }
		                    },
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    }
		                },
		                change: function () {
		                    var course = $("#txt-partID").data("kendoDropDownList");
		                    course.dataSource.filter({
		                        field: "departmentID",
		                        value: this.value(),
		                        operator: "eq"
		                    });
		                    course.value(0);
		                }
		            }).data("kendoDropDownList");
		            $("#txt-partID").kendoDropDownList({
		                optionLabel: "Select Part",
		                dataTextField: "name",
		                dataValueField: "partID",
		                headerTemplate: '<div class="dropdown-header">' +
                                            '<span class="k-widget k-header">Part Code</span>' +
                                            '<span class="k-widget k-header">Part Name</span>' +
                                        '</div>',
		                valueTemplate: '<span class="selected-value">#:data.code==undefined?"":data.code# #:data.name#</span>',
		                template: '<span class="k-state-default">#:data.code==undefined?"":data.code#</span>' +
                                  '<span class="k-state-default">#:data.name#</span>', dataSource: {
		                    type: "json",
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllPart' }),
		                            url: 'user.aspx'
		                        }
		                    },
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    }
		                },
		                change: function () {
		                    var course = $("#txt-subPartID").data("kendoDropDownList");
		                    course.dataSource.filter({
		                        field: "partID",
		                        value: this.value(),
		                        operator: "eq"
		                    });
		                    course.value(0);
		                }
		            }).data("kendoDropDownList");
		            $("#txt-subPartID").kendoDropDownList({
		                optionLabel: "Select Sub-Part",
		                dataTextField: "name",
		                dataValueField: "subPartID",
		                headerTemplate: '<div class="dropdown-header">' +
                                            '<span class="k-widget k-header">SubPart Code</span>' +
                                            '<span class="k-widget k-header">SubPart Name</span>' +
                                        '</div>',
		                valueTemplate: '<span class="selected-value">#:data.code==undefined?"":data.code# #:data.name#</span>',
		                template: '<span class="k-state-default">#:data.code==undefined?"":data.code#</span>' +
                                  '<span class="k-state-default">#:data.name#</span>', dataSource: {
		                    type: "json",
		                    transport: {
		                        read: {
		                            dataType: "json",
		                            type: "POST",
		                            data: ({ mode: 'selectAllSubPart' }),
		                            url: 'user.aspx'
		                        }
		                    },
		                    schema: {
		                        data: "data",
		                        total: "total"
		                    }
		                }
		            }).data("kendoDropDownList");

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

		                $('#txt-id').val(selectedItem.usersID);
		                $('#txt-firstname').val(selectedItem.firstname);
		                $('#txt-lastname').val(selectedItem.lastname);
		                $('#txt-username').val(selectedItem.username);
		                $('#txt-positionID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.positionID === selectedItem.positionID;
		                });
		                $('#txt-companyID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.companyID === selectedItem.companyID;
		                });
		                $('#txt-sectorID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.sectorID === selectedItem.sectorID;
		                });
		                $('#txt-departmentID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.departmentID === selectedItem.departmentID;
		                });
		                $('#txt-partID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.partID === selectedItem.partID;
		                });
		                $('#txt-subPartID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.subPartID === selectedItem.subPartID;
		                });
		                setCHKValue('chk-status', selectedItem.status);
		            });
		            $("#btn-delete").click(function () {
		                if ($("#btn-delete").prop("disabled")) return;
		                var list = $('#data-grid').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record to delete.'); return; }
		                if (confirm('Do you want to delete this record?')) {
		                    if (ajax('user.aspx', ({ mode: 'delete', id: selectedItem.usersID })) == 'true') {
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
