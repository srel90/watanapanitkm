﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sector.aspx.cs" Inherits="wattanapanitkm.sector" %>

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

            #txt-companyID-list .k-item > span {
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

            #txt-companyID-list .k-item > span:first-child {
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
                    <h1>Sector</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Sector</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Sector Management</h3>
								</div>
								<div class="panel-body">
									<div id="data-grid"></div>
									<br>
									<div class="panel panel-default">
										<div class="panel-heading">
										  <h3 class="panel-title">Sector Data</h3>
										</div>
										<div class="panel-body">
											<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="sector.aspx">
												<input type="hidden" id="txt-mode" name="mode" value="insert">
												<input type="hidden" id="txt-id" name="id">
                                                <div class="form-group">
													<label for="txt-companyID" class="col-sm-2 control-label">Company*</label>
													<div class="col-sm-3">
														<input id="txt-companyID" name="companyID" placeholder="Company" required validationMessage="Company is required!" style="width:400px">
													</div>
												</div>
												<div class="form-group">
												  <label for="txt-code" class="col-sm-2 control-label">Sector Code</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-code" name="code" placeholder="Sector code">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Sector Name</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-name" name="name" placeholder="Sector Name">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-detail" class="col-sm-2 control-label">Detail</label>
												  <div class="col-sm-3">
												  <textarea class="form-control" id="txt-detail" name="detail" placeholder="Detail"></textarea>
													</div>
												</div>
                                                <div class="form-group">
													  <label for="txt-headID" class="col-sm-2 control-label">Head user</label>
													  <div class="input-group col-sm-3">				
														<input type="hidden" id="txt-headID" name="headID" >
														<input type="text" class="form-control" id="txt-headName" name="headName" placeholder="Head user" readonly>
														<span class="input-group-btn">
														<button class="btn btn-primary" type="button" id="btn-headID"><i class="fa fa-search"></i></button>
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
					  <div class="modal fade" id="form-headUser" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
						<div class="modal-dialog modal-lg">
						  <div class="modal-content">
							<div class="modal-header">
							  <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							  <h4 class="modal-title" id="myModalLabel">Select head user</h4>
							</div>
							<div class="modal-body">
								<div id="data-grid-headUser"></div>
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
		                            data: ({ mode: 'selectAllSector' }),
		                            url: 'sector.aspx'
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
		                /*groupable: true,*/
		                resizable: true,
		                filterable: true,
		                reorderable: true,
		                sortable: true,
		                columnMenu: true,
		                selectable: "row",
		                pageable: { pageSizes: true, refresh: true },
		                columns: [
							{ field: "sectorID", title: "ID" },
                            { field: "companyName", title: "Company" },
							{ field: "code", title: "Code" },
                            { field: "name", title: "Name" },
                            { field: "detail", title: "Detail" },
                            { title: "Head user", template: '#:firstname# #:lastname#' },
                            { field: "status", title: "Status", template: '#=status==1?"Active":"Inactive"#' }
		                ],
		                toolbar: [
							{ template: '<button type="button" class="btn btn-primary" id="btn-add"><span class="glyphicon glyphicon-plus-sign"></span>Add new</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-edit"><span class="glyphicon glyphicon-edit"></span>Edit</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-delete"><span class="glyphicon glyphicon-remove-circle"></span>Delete</button>&nbsp;' },
							{ template: '<button type="button" class="btn btn-primary" id="btn-cancel"><span class="glyphicon glyphicon-repeat"></span>Cancel</button>' }
		                ]
		            });
		            $("#txt-companyID").kendoDropDownList({
		                optionLabel: "Select company...",
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
		                            url: 'sector.aspx'
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
		            $('#btn-headID').click(function () {
		                $("#data-grid-headUser").kendoGrid({
		                    dataSource: {
		                        transport: {
		                            read: {
		                                dataType: "json",
		                                type: "POST",
		                                data: ({ mode: 'selectAllHeadUser' }),
		                                url: 'sector.aspx'
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
								{ field: "usersID", title: "User ID"},
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
		                $("#data-grid").data("kendoGrid").dataSource.read();
		                $("#data-grid").data("kendoGrid").refresh();
		                $('#form-headUser').modal({ keyboard: false, backdrop: 'static' });
		            });
		            $('#btn-ok').click(function () {
		                var list = $('#data-grid-headUser').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record frist.'); return; }
		                $('#form-headUser').modal('hide');
		                $('#txt-headID').val(selectedItem.usersID);
		                $('#txt-headName').val(selectedItem.firstname + " " + selectedItem.lastname);
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
		                $('#txt-companyID').data("kendoDropDownList").select(function (dataItem) {
		                    return dataItem.companyID === selectedItem.companyID;
		                });
		                $('#txt-id').val(selectedItem.companyID);
		                $('#txt-code').val(selectedItem.code);
		                $('#txt-name').val(selectedItem.name);
		                $('#txt-detail').val(selectedItem.detail);
		                setCHKValue('chk-status', selectedItem.status);
		            });
		            $("#btn-delete").click(function () {
		                if ($("#btn-delete").prop("disabled")) return;
		                var list = $('#data-grid').data('kendoGrid');
		                var selectedItem = list.dataItem(list.select());
		                if (selectedItem == null) { alert('Please select record to delete.'); return; }
		                if (confirm('Do you want to delete this record?')) {
		                    if (ajax('sector.aspx', ({ mode: 'delete', id: selectedItem.sectorID })) == 'true') {
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
		            $("#data-grid-headUser").data("kendoGrid").dataSource.read({ search: $('#txt-search').val(), mode: 'searchHeadUser' });
		        }
		        this.validation = function () {
		            validator = $('#scriptForm').kendoValidator({
		                rules: {
		                    verifySectorCode: function (input) {
		                        var ret = true;
		                        if (input.is('#txt-code') && $('#txt-code').val() != "") {
		                            var cdata = ajax('sector.aspx', ({ mode: 'verifySectorCode', code: input.val() }), false);
		                            if ($.trim(cdata) == 'true') ret = false;
		                        }
		                        if ($('#txt-mode').val() == 'update') ret = true;
		                        return ret;
		                    },
		                    //requireSectorCode: function (input) {
		                    //    var ret = true;
		                    //    if (input.is('#txt-code')) {
		                    //        if ($.trim(input.val()) == '') ret = false;
		                    //    }
		                    //    return ret;
		                    //}
		                },
		                messages: {
		                    verifySectorCode: "This sector is already exist!",
		                    //requireSectorCode: "Sector code is required!",
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
		            $('#txt-headID').val('');
		            
		        }//end clearForm
		    }
		</script>
    </body>
</html>