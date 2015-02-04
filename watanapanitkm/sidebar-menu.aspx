<ul class="sidebar-menu">
  <li <% Response.Write(wattanapanitkm.utility.GetCurrentPageName() == "main.aspx"? "class=\"active\"" : ""); %>>
    <a href="main.aspx">
      <i class="fa fa-search"></i>
      <span>Search</span>
    </a>
  </li>
  <%
    String active="";
	switch( wattanapanitkm.utility.GetCurrentPageName()){
      case "company.aspx":active="active";break;
      case "sector.aspx":active="active";break;
      case "department.aspx":active="active";break;
      case "part.aspx":active="active";break;
      case "subpart.aspx":active="active";break;
      case "position.aspx":active="active";break;
      case "importcompany.aspx":active="active";break;
      case "importsector.aspx":active="active";break;
      case "importdepartment.aspx":active="active";break;
      case "importpart.aspx":active="active";break;
      case "importsubpart.aspx":active="active";break;
      case "importpositon.aspx":active="active";break;
      case "importuser.aspx":active="active";break;
      case "importjtda.aspx":active="active";break;
      case "importemployee.aspx":active="active";break;
      default:active="";break;
      }
 if(((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString().Equals("admin")){ 
    %>
  <li class="treeview <% Response.Write(active);%>">
    <a href="#">
      <i class="fa fa-info-circle"></i>
      Basic Information
      <i class="fa fa-angle-left pull-right"></i>
    </a>
    <ul class="treeview-menu" >
      <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="company.aspx"?"class=\"active\"":"");%>><a href="company.aspx">
          <i class="fa fa-angle-double-right"></i> Company
        </a>
      </li>
      <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="sector.aspx"?"class=\"active\"":"");%>><a href="sector.aspx">
          <i class="fa fa-angle-double-right"></i> Sector
        </a>
      </li>
      <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="department.aspx"?"class=\"active\"":"");%>><a href="department.aspx">
          <i class="fa fa-angle-double-right"></i> Department
        </a>
      </li>
      <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="part.aspx"?"class=\"active\"":"");%>><a href="part.aspx">
          <i class="fa fa-angle-double-right"></i> Part
        </a>
      </li>
      <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="subpart.aspx"?"class=\"active\"":"");%>><a href="subpart.aspx">
          <i class="fa fa-angle-double-right"></i> Sub-Part
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="position.aspx"?"class=\"active\"":"");%>><a href="position.aspx">
          <i class="fa fa-angle-double-right"></i> Position
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importcompany.aspx"?"class=\"active\"":"");%>><a href="importcompany.aspx">
          <i class="fa fa-angle-double-right"></i> Import company
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importsector.aspx"?"class=\"active\"":"");%>><a href="importsector.aspx">
          <i class="fa fa-angle-double-right"></i> Import sector
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importdepartment.aspx"?"class=\"active\"":"");%>><a href="importdepartment.aspx">
          <i class="fa fa-angle-double-right"></i> Import department
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importpart.aspx"?"class=\"active\"":"");%>><a href="importpart.aspx">
          <i class="fa fa-angle-double-right"></i> Import part
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importsubpart.aspx"?"class=\"active\"":"");%>><a href="importsubpart.aspx">
          <i class="fa fa-angle-double-right"></i> Import subpart
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importposition.aspx"?"class=\"active\"":"");%>><a href="importposition.aspx">
          <i class="fa fa-angle-double-right"></i> Import position
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importuser.aspx"?"class=\"active\"":"");%>><a href="importuser.aspx">
          <i class="fa fa-angle-double-right"></i> Import JTDA Users DATA
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importjtda.aspx"?"class=\"active\"":"");%>><a href="importjtda.aspx">
          <i class="fa fa-angle-double-right"></i> Import JTDA DATA
        </a>
      </li>
    <li
        <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="importemployee.aspx"?"class=\"active\"":"");%>><a href="importemployee.aspx">
          <i class="fa fa-angle-double-right"></i> Import employee DATA
        </a>
      </li>
    </ul>
  </li>
  <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="user.aspx"?"class=\"active\"":"");%>>
    <a href="user.aspx">
      <i class="fa fa-users"></i>
      <span>Users JTDA</span>
    </a>
  </li>
   <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="employee.aspx"?"class=\"active\"":"");%>>
    <a href="employee.aspx">
      <i class="fa fa-users"></i>
      <span>Employee</span>
    </a>
  </li>
 <% } %>
  <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="jtda.aspx"?"class=\"active\"":"");%>>
    <a href="jtda.aspx">
      <i class="fa fa-lightbulb-o"></i>
      <span>Solution Data (JTDA)</span>
    </a>
  </li>
  <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="download.aspx"?"class=\"active\"":"");%>>
    <a href="download.aspx">
      <i class="fa fa-download"></i>
      <span>Download</span>
    </a>
  </li>
  <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="link.aspx"?"class=\"active\"":"");%>>
    <a href="link.aspx">
      <i class="fa fa-external-link"></i>
      <span>Link</span>
    </a>
  </li>
    <% if(Session["headposition_taskover"]!=null){ 
 %>
  <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="report1.aspx"?"class=\"active\"":"");%>>
    <a href="report1.aspx">
      <i class="fa fa-external-link"></i>
      <span>JTDA Task report</span>
    </a>
  </li>
    <% } %>
    <li
   <% Response.Write(wattanapanitkm.utility.GetCurrentPageName()=="listquestion.aspx"?"class=\"active\"":"");%>>
    <a href="listquestion.aspx">
      <i class="fa fa-external-link"></i>
      <span>Web Board</span>
    </a>
  </li>
</ul>