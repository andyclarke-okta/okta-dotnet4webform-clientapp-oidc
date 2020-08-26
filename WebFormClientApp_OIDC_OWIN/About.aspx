<%@ Page Title="Protected" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebFormClientApp_OIDC_OWIN.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Protected Content</h3>
    <p>You must be authenticted and authorized via Okta to view this page</p>
</asp:Content>
