<nav class="navbar navbar-static-top" role="navigation">
  <!-- Sidebar toggle button-->
  <a href="#" class="navbar-btn sidebar-toggle" data-toggle="offcanvas" role="button">
    <span class="sr-only">Toggle navigation</span>
    <span class="icon-bar"></span>
    <span class="icon-bar"></span>
    <span class="icon-bar"></span>
  </a>
  <div class="navbar-right">
    <ul class="nav navbar-nav">
      <!-- Messages: style can be found in dropdown.less-->
        <% if(Session["headposition_taskover"]!=null){ 
           System.Data.DataTable dt=((System.Data.DataTable)Session["headposition_taskover"]);    
                %>
      <li class="dropdown messages-menu">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
          <i class="fa fa-envelope"></i>
          <span class="label label-success"><% Response.Write(dt.Rows.Count>99?"99+":dt.Rows.Count.ToString());%></span>
        </a>
        <ul class="dropdown-menu">
          <li class="header">You have <% Response.Write(dt.Rows.Count);%> alert</li>
          <li>
            <!-- inner menu: contains the actual data -->
            <ul class="menu">
                <% 
                
                for(int i=0;i<dt.Rows.Count;i++){ %>
              <li>
                <!-- start message -->
                <a href="#">
                  <h4 style="margin: 0!important;">
                    <% Response.Write(dt.Rows[i]["docNo"].ToString()); %>
                    <small class="pull-right"><% Response.Write(dt.Rows[i]["workProgress"].ToString()); %>%</small>
                  </h4>
                  <p style="margin: 0!important;white-space: normal;"><% Response.Write(dt.Rows[i]["title"].ToString()); %></p>
                    <div class="progress xs">
                                                    <div class="progress-bar progress-bar-aqua" style="width: <% Response.Write(dt.Rows[i]["workProgress"].ToString()); %>%" role="progressbar" aria-valuenow="<% Response.Write(dt.Rows[i]["workProgress"].ToString()); %>" aria-valuemin="0" aria-valuemax="100">
                                                        <span class="sr-only"><% Response.Write(dt.Rows[i]["workProgress"].ToString()); %>% Complete</span>
                                                    </div>
                                                </div>
                </a>
              </li>
                <% } %>
              <!-- end message -->
              
            </ul>
          </li>
          <li class="footer">
            <a href="report1.aspx">See All Alert</a>
          </li>
        </ul>
      </li>
<% } %>
      <!-- User Account: style can be found in dropdown.less -->
      <li class="dropdown user user-menu">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
          <i class="glyphicon glyphicon-user"></i>
          <span>
            <?php echo $_SESSION['user'][0]['name'];?>
            <i class="caret"></i>
          </span>
        </a>
        <ul class="dropdown-menu">
          <!-- User image -->
          <li class="user-header bg-light-blue">
            <img src="img/avatar.png" class="img-circle" alt="User Image" />
            <p>
              <% Response.Write(((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString());%><br>
                                       
                                    </p>
          </li>
          <!-- Menu Body -->
          <!--<li class="user-body">
                                    <div class="col-xs-4 text-center">
                                        <a href="#">Followers</a>
                                    </div>
                                    <div class="col-xs-4 text-center">
                                        <a href="#">Sales</a>
                                    </div>
                                    <div class="col-xs-4 text-center">
                                        <a href="#">Friends</a>
                                    </div>
                                </li>-->
          <!-- Menu Footer-->
          <li class="user-footer">
            <div class="pull-left">
              <a href="updateemployee.aspx" class="btn btn-default btn-flat">Profile</a>
            </div>
            <div class="pull-right">
              <a href="logout.aspx" class="btn btn-default btn-flat">Sign out</a>
            </div>
          </li>
        </ul>
      </li>
    </ul>
  </div>
</nav>

