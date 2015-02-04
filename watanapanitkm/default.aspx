<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="wattanapanitkm._default" %>

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


		<!-- Custom styles for this template -->
		<style>
			body {
			  padding-top: 40px;
			  padding-bottom: 40px;
			  background-color: #eee;
			}

			.form-signin {
			  max-width: 330px;
			  padding: 15px;
			  margin: 0 auto;
			}
			.form-signin .form-signin-heading,
			.form-signin .checkbox {
			  margin-bottom: 10px;
			}
			.form-signin .checkbox {
			  font-weight: normal;
			}
			.form-signin .form-control {
			  position: relative;
			  height: auto;
			  -webkit-box-sizing: border-box;
				 -moz-box-sizing: border-box;
					  box-sizing: border-box;
			  padding: 10px;
			  font-size: 16px;
			}
			.form-signin .form-control:focus {
			  z-index: 2;
			}
			.form-signin input[type="email"] {
			  margin-bottom: -1px;
			  border-bottom-right-radius: 0;
			  border-bottom-left-radius: 0;
			}
			.form-signin input[type="password"] {
			  margin-bottom: 10px;
			  border-top-left-radius: 0;
			  border-top-right-radius: 0;
			}
		</style>

		<!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
		<!--[if lt IE 9]>
		  <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
		  <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
		<![endif]-->
	</head>

	<body>
		<div class="container">
            <h2 class="form-signin-heading" style="text-align: center"><span class="form-signin-heading" style="text-align: center">
                <img src="img/wattanapanit.png" width="200" height="128"></span><br>
                <img src="img/reservation-icon.png" width="64" height="64">Login to Wattapanit Knowledge Management<br>
            </h2>
		  
            <form action="default.aspx" method="post" class="form-signin" id="form-signin" role="form">
                <input id="mode" name="mode" type="hidden" value="checkLogin" />

                <input name="username" type="text" class="form-control" placeholder="Username" required autofocus>
                <input name="password" type="password" class="form-control" placeholder="Password" required>

                <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
                <div align="center" id="loading">
                    <img src="img/loading.gif"></div>
                <div align="center" class="alert alert-warning" id="alert">Username or Password is invalid!</div>
            </form>
		</div>
	<!-- /container --> 

	<!-- Bootstrap core JavaScript
		================================================== --> 
	<!-- Placed at the end of the document so the pages load faster --> 
			<script src="javascripts/jquery-2.1.1.min.js"></script>
			<script src="javascripts/jquery-ui.min.js" type="text/javascript"></script>
			<script src="javascripts/jquery.form.min.js" type="text/javascript"></script>
			<script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script> 
			<script src="javascripts/utility.js" type="text/javascript" ></script> 
			<script type="text/javascript">
			    $(function () {
			        script.initial();
			        script.eventhandle();
			    });
			    var script = new function () {
			        this.initial = function () {
			            $('#loading,#alert').hide();
			        }
			        this.eventhandle = function () {
			            $('#form-signin').submit(function (e) {
			                e.preventDefault();
			                script.login();
			            });
			        }
			        this.login = function () {
			            var options = {
			                success: function (response) {
			                    $('#loading').hide();
			                    if ($.trim(response) == 'true') {
			                        _Redirect('main.aspx');
			                    } else {
			                        $("#alert").hide().fadeIn("slow");
			                    }
			                }
			            };
			            $('#loading').show();
			            $("#form-signin").ajaxSubmit(options);
			        }
			    }
			</script>
	</body>
</html>

