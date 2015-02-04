<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="wattanapanitkm.company" %>

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
                    <h1>Company</h1>
                    <ol class="breadcrumb">
                        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
                        <li class="active">Company</li>
                    </ol>
                </section>
                <!-- Main content -->
                <section class="content">
					<div class="row">
						<div class="col-md-12">
							<div class="panel panel-default">
								<div class="panel-heading">
								<h3 class="panel-title">Company Management</h3>
								</div>
								<div class="panel-body">
									<div id="data-grid"></div>
									<br>
									<div class="panel panel-default">
										<div class="panel-heading">
										  <h3 class="panel-title">Company Data</h3>
										</div>
										<div class="panel-body">
											<form class="form-horizontal" role="form" id="scriptForm" name="scriptForm" method="post" action="company.aspx">
												<input type="hidden" id="txt-mode" name="mode" value="insert">
												<input type="hidden" id="txt-id" name="id">
												<div class="form-group">
												  <label for="txt-code" class="col-sm-2 control-label">Company Code</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-code" name="code" placeholder="Company code">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-name" class="col-sm-2 control-label">Company Name</label>
												  <div class="col-sm-3">
													<input type="text" class="form-control" id="txt-name" name="name" placeholder="Company Name">
													</div>
												</div>
                                                <div class="form-group">
												  <label for="txt-detail" class="col-sm-2 control-label">Detail</label>
												  <div class="col-sm-3">
												  <textarea class="form-control" id="txt-detail" name="detail" placeholder="Detail"></textarea>
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
		                            data: ({ mode: 'selectAllCompany' }),
		                            url: 'company.aspx'
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
							{ field: "companyID", title: "ID" },
							{ field: "code", title: "Code" },
                            { field: "name", title: "Name" },
                            { field: "detail", title: "Detail" },
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
		                    if (ajax('company.aspx', ({ mode: 'delete', id: selectedItem.companyID })) == 'true') {
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
		                    verifyCompanyCode: function (input) {
		                        var ret = true;
		                        if (input.is('#txt-code')&&$('#txt-code').val()!="") {
		                            var cdata = ajax('company.aspx', ({ mode: 'verifyCompanyCode', code: input.val() }), false);
		                            if ($.trim(cdata) == 'true') ret = false;
		                        }
		                        if ($('#txt-mode').val() == 'update') ret = true;
		                        return ret;
		                    },
		                    //requireCompanyCode: function (input) {
		                    //    var ret = true;
		                    //    if (input.is('#txt-code')) {
		                    //        if ($.trim(input.val()) == '') ret = false;
		                    //    }
		                    //    return ret;
		                    //}
		                },
		                messages: {
		                    //verifyCompanyCode: "This company is already exist!",
		                    requireCompanyCode: "Company code is required!",
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
