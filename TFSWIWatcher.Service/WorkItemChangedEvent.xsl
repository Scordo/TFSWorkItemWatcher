<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html"/>
<!--
This style sheet contains common formatting and handling elements for Team Foundation. 

Templates:

    footer (format, alertOwner, timeZoneName, timeZoneOffset)

    link(format, link, displayText):
      Generates a link
    where
        format: html or text
        link: Url
        displayText:
-->

<!-- Common strings -->
<xsl:variable name="morePrompt" select="'More ...'"/>
<xsl:variable name="FileType" select="'File'"/>
<xsl:variable name="FolderType" select="'Folder'"/>
<xsl:variable name="datetime" select="'- All dates and times are shown in GMT '"/>
<xsl:variable name="tfUrl" select="'http://msdn.microsoft.com/vstudio/teamsystem/'"/>
<xsl:variable name="tmText" select="'Microsoft Visual Studio&#174; Team System 2008'"/>
<xsl:variable name="textSeparatorLong" select="'----------------------------------------------------------------------'"/>
<xsl:variable name="by" select="'Provided by '"/>
<xsl:variable name="reason" select="'- You are receiving this notification because of a subscription created by '"/>
<xsl:variable name="Error" select="'An error occured while your request was being processed. This may be a transient error and retrying the request may help. If this error persists or you feel it warrants additional attention, please provide this message and the error message(s) that appear to your administrative staff'"/>
<xsl:variable name="DetailedErrorHeader" select="'Detailed error message(s) (for administrative staff):'"/>
<!-- Item information webview title -->
<xsl:variable name="ChangesetViewTitle" select="'Changeset Information'"/>

<!-- Footer -->
<xsl:template name="footer">
<xsl:param name="format"/>
<xsl:param name="alertOwner"/>
<xsl:param name="timeZoneName"/>
<xsl:param name="timeZoneOffset"/>
<xsl:choose>
    <xsl:when test="$format='html'">
        <div class="footer">
        <br/>
        Notes:
        <br/>
        <xsl:if test="string-length($timeZoneName) > 0">
            <xsl:value-of select="$datetime"/>
            <xsl:value-of select="$timeZoneOffset"/>
            <xsl:value-of select="concat(' ', $timeZoneName)"/>
            <br/>
        </xsl:if>
        <xsl:if test="string-length($alertOwner) > 0">
            <xsl:value-of select="$reason"/>
            <xsl:value-of select="$alertOwner"/>
            <br/>
        </xsl:if>
        <xsl:value-of select="$by"/>
        <xsl:call-template name="link">
            <xsl:with-param name="format" select="$format"/>
            <xsl:with-param name="link" select="$tfUrl"/>
            <xsl:with-param name="displayText" select="$tmText"/>
        </xsl:call-template>
        </div>
    </xsl:when>
<xsl:when test="$format='text'">
<xsl:text>&#xA;--------------- NOTES ----------------&#xA;</xsl:text>
<xsl:if test="string-length($timeZoneName) > 0">
<xsl:value-of select="$datetime"/><xsl:value-of select="$timeZoneOffset"/><xsl:value-of select="concat(' ', $timeZoneName)"/>
<xsl:text>&#xA;</xsl:text>
</xsl:if>
<xsl:if test="string-length($alertOwner) > 0">
<xsl:value-of select="$reason"/>
<xsl:value-of select="$alertOwner"/>
<xsl:text>&#xA;</xsl:text>
</xsl:if>
<xsl:value-of select="$by"/>
<xsl:call-template name="link">
<xsl:with-param name="format" select="$format"/>
<xsl:with-param name="link" select="$tfUrl"/>
<xsl:with-param name="displayText" select="$tmText"/>
</xsl:call-template>
    </xsl:when>
</xsl:choose>
</xsl:template> <!-- footer -->

<xsl:template name="link">
    <xsl:param name="format"/>
    <xsl:param name="link"/>
    <xsl:param name="embolden"/>
    <xsl:param name="fontSize"/>
    <xsl:param name="displayText"/>
    <xsl:choose>
        <xsl:when test="$format='html'">
            <a>
                <xsl:attribute name="HREF">
                    <xsl:value-of select="$link"/>
                </xsl:attribute>
                <xsl:attribute name="title">
                    <xsl:value-of select="$displayText"/>
                </xsl:attribute>
                <xsl:choose>
                    <xsl:when test="$embolden='true'">
                        <b>
                        <xsl:value-of select="$displayText"/>
                        </b>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="$displayText"/>
                    </xsl:otherwise>
                </xsl:choose>
            </a>
        </xsl:when>
<xsl:when test="$format='text'">
<xsl:value-of select="$displayText"/><xsl:value-of disable-output-escaping="yes" select="concat(' (',$link,')')"/>
</xsl:when>
    </xsl:choose>
    </xsl:template>
<xsl:template name="style">
  <STYLE TYPE="text/css">
  body, input, button
  {
    color: black;
    font-family: Verdana,Arial,Helvetica;
    font-size: x-small;
  }
  p
  {
    color: #666666;
  }
  h1
  {
    color: #666666;
    font-size: medium;
  }
  h2
  {
    color: black;
  }
  table
  {
    border-collapse: collapse;
    border-width: 0;
    border-spacing: 0;
    width: 90%;
  }
  pre
  {
    word-wrap: break-word;
    font-size: x-small;
    font-family: Verdana,Arial,Helvetica;
    display: inline;
  }
  table.WithBorder
  {
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    border-collapse: collapse;
    width: 90%;
  }
  TD
  {
    vertical-align: top;
    font-size: x-small;
  }
  TD.PropValue
  {
    font-size: x-small;
    width: auto;
  }
  TD.Col1Data
  {
    font-size: x-small;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    background: #F9F8F4;
    width: auto;
  }
  TD.ColData
  {
    font-size: x-small;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
  }
  TD.ColDataXSmall
  {
    font-size: x-small;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    width: 5%;
  }
  TD.ColDataSmall
  {
    font-size: x-small;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    width: 10%;
  }
  TD.ColHeadingXSmall
  {
    background-color: #F1EFE2;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    font-size: x-small;
    width: 5%;
  }
  TD.ColHeadingSmall
  {
    background-color: #F1EFE2;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    font-size: x-small;
    width: 10%;
  }
  TD.ColHeadingMedium
  {
    background: #F1EFE2;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    font-size: x-small;
    width: 200px;
  }
  TD.ColHeading
  {
    font-size: x-small;
    border-style: solid;
    border-color: #F1EFE2;
    border-width: 1px;
    background: #F1EFE2;
    width: auto;
  }
  .Title
  {
    width:100%;
    font-size: medium;
  }
  .footer
  {
    width:100%;
    font-size: xx-small;
  }
  </STYLE>
</xsl:template>

<!-- Team Foundation XSL templates -->
<xsl:template name="TeamFoundationItem">
<xsl:param name="format"/>
<xsl:param name="Item"/>
<xsl:choose>
    <xsl:when test="$format='html'">
    <xsl:if test="string-length(Item/@title) > 0">
        <title><xsl:value-of select="Item/@title"/></title>
        <div class="Title"><xsl:value-of select="Item/@title"/></div>
    </xsl:if>
    <xsl:if test="string-length(Item/@title) = 0">
        <title><xsl:value-of select="Item/@item"/></title>
        <div class="Title"><xsl:value-of select="Item/@item"/></div>
    </xsl:if>
    <b>Summary</b>
    <table>
        <tr>
            <td>Server Path:</td>
            <td class="PropValue"><xsl:value-of select="Item/@item"/></td>
        </tr>
        <tr>
            <td>Changeset:</td>
            <td class="PropValue">
            <xsl:if test="string-length(Item/@csurl) > 0">
                <xsl:call-template name="link">
                    <xsl:with-param name="format" select="'html'"/>
                    <xsl:with-param name="link" select="Item/@csurl"/>
                    <xsl:with-param name="displayText" select="Item/@cs"/>
                </xsl:call-template>
            </xsl:if>
            <xsl:if test="string-length(Item/@csurl) = 0">
                <xsl:value-of select="Item/@cs"/>
            </xsl:if>
            </td>
        </tr>
        <tr>
            <td>Last Modified On:</td>
            <td class="PropValue"><xsl:value-of select="Item/@date"/></td>
        </tr>
        <tr>
            <td>Type:</td>
            <td class="PropValue">
                <xsl:if test="Item/@type = 'File'">
                    <xsl:value-of select="$FileType"/>
                </xsl:if>
                <xsl:if test="Item/@type = 'Folder'">
                    <xsl:value-of select="$FolderType"/>
                </xsl:if>
            </td>
        </tr>
        <xsl:if test="Item/@type = 'File'">
        <tr>
            <td>File Size (bytes):</td>
            <td class="PropValue"><xsl:value-of select="Item/@len"/></td>
        </tr>
        </xsl:if>
    </table>
    </xsl:when>
    <xsl:when test="$format='text'">
<xsl:if test="string-length(Item/@title) > 0">
<xsl:value-of select="Item/@title"/>
</xsl:if>
<xsl:if test="string-length(Item/@title) = 0">
<xsl:value-of select="Item/@item"/>
</xsl:if>

Summary
Server Path:       <xsl:value-of select="Item/@item"/>
Changeset:         <xsl:if test="string-length(Item/@csurl) > 0">
<xsl:call-template name="link">
<xsl:with-param name="format" select="'text'"/>
<xsl:with-param name="link" select="Item/@csurl"/>
<xsl:with-param name="displayText" select="Item/@cs"/>
</xsl:call-template>
</xsl:if>
<xsl:if test="string-length(Item/@csurl) = 0">
<xsl:value-of select="Item/@cs"/>
</xsl:if>
Last Modified On:  <xsl:value-of select="Item/@date"/>
<xsl:if test="Item/@type = 'File'">
Type:              <xsl:value-of select="$FileType"/>
</xsl:if>
<xsl:if test="Item/@type = 'Folder'">
Type:              <xsl:value-of select="$FolderType"/>
</xsl:if>
<xsl:if test="@type = 'File'">
File Size (bytes): <xsl:value-of select="Item/@len"/>
</xsl:if>
</xsl:when>
</xsl:choose>
<xsl:call-template name="footer">
   <xsl:with-param name="format" select="$format"/>
   <xsl:with-param name="timeZoneOffset" select="Item/@tzo"/>
   <xsl:with-param name="timeZoneName" select="Item/@tz"/>
</xsl:call-template>
</xsl:template>

<!-- Checkin event -->
<xsl:template name="CheckinEvent">
<xsl:param name="CheckinEvent"/>
<head>
    <title><xsl:value-of select="Title"/></title>
<div class="Title">
<xsl:call-template name="link">
  <xsl:with-param name="format" select="'html'"/>
  <xsl:with-param name="embolden" select="'true'"/>
  <xsl:with-param name="fontSize" select="'larger'"/>
  <xsl:with-param name="link" select="Artifacts/Artifact[@ArtifactType='Changeset']/Url"/>
  <xsl:with-param name="displayText" select="ContentTitle"/>
</xsl:call-template>
<!-- Pull in the command style settings -->
<xsl:call-template name="style">
</xsl:call-template>
</div>
</head>
<body lang="EN-US" link="blue" vlink="purple">
<!-- Display the summary message -->
<xsl:if test="string-length(Notice) > 0">
    <br/>
    <h1>
    <xsl:value-of select="Notice"/>
    </h1>
</xsl:if>
<br/>
<b>Summary</b>
<table style="table-layout: fixed">
<tr>
<td>Team Project(s):</td>
<td class="PropValue">
    <xsl:value-of select="TeamProject"/>
</td>
</tr>
<tr>
<xsl:choose>
    <xsl:when test="Owner != Committer">
        <td>Checked in on behalf of:</td>
    </xsl:when>
    <xsl:when test="Owner = Committer">
        <td>Checked in by:</td>
    </xsl:when>
</xsl:choose>
<td class="PropValue">
    <xsl:variable name="owner" select="substring-after(Owner,'\')"/>
    <xsl:if test="$owner=''">
        <xsl:value-of select="Owner"/>
    </xsl:if>
    <xsl:if test="$owner!=''">
        <xsl:value-of select="$owner"/>
    </xsl:if>
</td>
</tr>
<xsl:if test="string-length(Committer) > 0">
    <!-- only print if commiter != owner ) -->
    <xsl:if test="Owner != Committer">
    <tr>
        <td>Checked in by:</td>
        <td class="PropValue">
        <xsl:variable name="cmtr" select="substring-after(Committer,'\')"/>
        <xsl:if test="$cmtr=''">
            <xsl:value-of select="Committer"/>
        </xsl:if>
        <xsl:if test="$cmtr!=''">
            <xsl:value-of select="$cmtr"/>
        </xsl:if>
        </td>
    </tr>
    </xsl:if>
</xsl:if>
<tr>
<td>Checked in on:</td>
<td class="PropValue">
  <xsl:value-of select="CreationDate"/>
</td>
</tr>
<!-- Add the checkin notes, if present -->
<xsl:for-each select="CheckinNotes/CheckinNote">
<tr>
  <td><xsl:value-of select="concat(@name,':')"/></td>
  <td class="PropValue">
    <xsl:variable name="valueLength" select="string-length(@val)"/>
    <xsl:if test="$valueLength > 0"><pre><xsl:value-of select="@val"/></pre></xsl:if>
    <xsl:if test="$valueLength = 0"><pre>None</pre></xsl:if>
  </td>
</tr>
</xsl:for-each>
<tr>
<td>Comment:</td>
<td class="PropValue">
  <xsl:variable name="commentLength" select="string-length(Comment)"/>
  <xsl:if test="$commentLength > 0">
    <pre>
        <xsl:value-of select="Comment"/>
    </pre>
  </xsl:if>
  <xsl:if test="$commentLength = 0"><pre>None</pre></xsl:if>
</td>
  </tr>
<!-- Optional policy override comment -->
<xsl:if test="string-length(PolicyOverrideComment) > 0">
 <tr>
 <td>Policy Override Reason:</td>
<td class="PropValue">
    <pre><xsl:value-of select="PolicyOverrideComment"/></pre>
</td>
 </tr>
</xsl:if>
</table>
<!-- Add the work item information, if present -->
<xsl:if test="count(CheckinInformation/CheckinInformation[@CheckinAction='Resolve']) > 0">
<br/>
<b>Resolved Work Items</b>
<table class="WithBorder">
<tr>
  <td class="ColHeadingSmall">Type</td>
  <td class="ColHeadingXSmall">ID</td>
  <td class="ColHeading">Title</td>
  <td class="ColHeadingSmall">Status</td>
  <td class="ColHeadingSmall">Assigned To</td>
</tr>
<xsl:for-each select="CheckinInformation/CheckinInformation[@CheckinAction='Resolve']">
  <tr>
      <xsl:call-template name="wiItem">
          <xsl:with-param name="Url" select="@Url"/>
          <xsl:with-param name="Type" select="@Type"/>
          <xsl:with-param name="Id" select="@Id"/>
          <xsl:with-param name="Title" select="@Title"/>
          <xsl:with-param name="State" select="@State"/>
          <xsl:with-param name="AssignedTo" select="@AssignedTo"/>
      </xsl:call-template>
  </tr>
</xsl:for-each>
</table>
</xsl:if> <!-- Resolved WI Info -->
<!-- Add the Associated work item information, if present -->
<xsl:if test="count(CheckinInformation/CheckinInformation[@CheckinAction='Associate']) > 0">
<br/>
<b>Associated Work Items</b>
<table class="WithBorder">
<tr>
  <td class="ColHeadingSmall">Type</td>
  <td class="ColHeadingXSmall">ID</td>
  <td class="ColHeading">Title</td>
  <td class="ColHeadingSmall">Status</td>
  <td class="ColHeadingSmall">Assigned To</td>
</tr>
<xsl:for-each select="CheckinInformation/CheckinInformation[@CheckinAction='Associate']">
  <tr>
      <xsl:call-template name="wiItem">
          <xsl:with-param name="Url" select="@Url"/>
          <xsl:with-param name="Type" select="@Type"/>
          <xsl:with-param name="Id" select="@Id"/>
          <xsl:with-param name="Title" select="@Title"/>
          <xsl:with-param name="State" select="@State"/>
          <xsl:with-param name="AssignedTo" select="@AssignedTo"/>
      </xsl:call-template>
  </tr>
</xsl:for-each>
</table>
</xsl:if> <!-- Associated WI Info -->
<!-- Add policy failures, if present -->
<xsl:if test="count(PolicyFailures/PolicyFailure) > 0">
<br/>
<b>Policy Failures</b>
<table class="WithBorder">
<tr>
  <td class="ColHeading">Type</td>
  <td class="ColHeading">Description</td>
</tr>
<xsl:for-each select="PolicyFailures/PolicyFailure">
  <tr>
    <td class="ColData">
      <xsl:value-of select="@name"/>
  </td>
    <td class="ColData">
      <xsl:variable name="valueLength" select="string-length(@val)"/>
      <xsl:if test="$valueLength > 0"><xsl:value-of select="@val"/></xsl:if>
      <xsl:if test="$valueLength = 0"><pre>None</pre></xsl:if>
    </td>
  </tr>
</xsl:for-each>
</table>
</xsl:if>
<!-- Add the versioned items -->
<xsl:if test="count(Artifacts/Artifact[@ArtifactType='VersionedItem']) > 0">
<br/>
<b>Items</b>
<table class="WithBorder">
<tr>
<td class="ColHeading">Name</td>
<td class="ColHeadingSmall">Change</td>
<td class="ColHeading">Folder</td>
</tr>
<xsl:for-each select="Artifacts/Artifact[@ArtifactType='VersionedItem']">
<tr>
  <td class="ColData">
      <xsl:call-template name="link">
        <xsl:with-param name="format" select="'html'"/>
        <xsl:with-param name="link" select="Url"/>
        <xsl:with-param name="displayText" select="@Item"/>
      </xsl:call-template>
  </td> 
  <td class="ColDataSmall"><xsl:value-of select="@ChangeType"/></td>
  <td class="ColData"><xsl:value-of select="@Folder"/></td>
</tr>
</xsl:for-each>
<xsl:if test="AllChangesIncluded = 'false'">
<tr>
  <td class="ColData">
      <xsl:call-template name="link">
        <xsl:with-param name="format" select="'html'"/>
        <xsl:with-param name="link" select="Artifacts/Artifact[@ArtifactType='Changeset']/Url"/>
        <xsl:with-param name="displayText" select="$morePrompt"/>
      </xsl:call-template>
  </td> 
  <td class="ColDataSmall"><xsl:value-of select="' '"/></td>
  <td class="ColData"><xsl:value-of select="' '"/></td>
</tr>
</xsl:if>
</table>
</xsl:if> <!-- if there are versioned items -->
<xsl:call-template name="footer">
<xsl:with-param name="format" select="'html'"/>
<xsl:with-param name="alertOwner" select="Subscriber"/>
<xsl:with-param name="timeZoneOffset" select="TimeZoneOffset"/>
<xsl:with-param name="timeZoneName" select="TimeZone"/>
</xsl:call-template>
</body>

</xsl:template> <!-- checkin event -->

<!-- Workitem (Source Code Control View) -->
<xsl:template name="wiItem">
    <xsl:param name="Url"/>
    <xsl:param name="Type"/>
    <xsl:param name="Id"/>
    <xsl:param name="Title"/>
    <xsl:param name="State"/>
    <xsl:param name="AssignedTo"/>
    <td class="ColDataSmall"><xsl:value-of select="@Type"/></td>
    <td class="ColDataXSmall" >
      <xsl:call-template name="link">
        <xsl:with-param name="format" select="'html'"/>
        <xsl:with-param name="link" select="@Url"/>
        <xsl:with-param name="displayText" select="@Id"/>
      </xsl:call-template>
    </td> 
    <td class="ColData"><xsl:value-of select="@Title"/></td>
    <td class="ColDataSmall"><xsl:value-of select="@State"/></td>
    <td class="ColDataSmall">
      <xsl:variable name="assignedToLength" select="string-length(@AssignedTo)"/>
      <xsl:if test="$assignedToLength > 0"><xsl:value-of select="@AssignedTo"/></xsl:if>
      <xsl:if test="$assignedToLength = 0">N/A</xsl:if>
    </td>
</xsl:template> <!-- wiItem -->

<!-- Handle exceptions -->
<xsl:template name="Exception">
    <xsl:param name="format"/>
    <xsl:param name="Exception"/>
    <xsl:if test="$format='html'">
        <div class="Title">
        <xsl:value-of select="$Error"/>
        </div>
        <br/>
        <xsl:value-of select="$DetailedErrorHeader"/>
        <br/>
        <pre><xsl:value-of select="$Exception/Message"/></pre>
        <xsl:call-template name="footer">
          <xsl:with-param name="format" select="'html'"/>
        </xsl:call-template>
    </xsl:if>
    <xsl:if test="$format='text'">
    <xsl:value-of select="$Error"/>
    <xsl:text>&#xA;</xsl:text>
    <xsl:value-of select="$DetailedErrorHeader"/>
    <xsl:text>&#xA;</xsl:text>
    <xsl:value-of select="$Exception/Message"/>
    </xsl:if>
</xsl:template>

  <xsl:template match="WorkItemChangedEvent">
      <head>
      <!-- Pull in the command style settings -->
      <xsl:call-template name="style">
      </xsl:call-template>
      </head>
      <body>
          <div class="Title"><b>
            <xsl:choose>
              <xsl:when test="DisplayUrl[.!='']">
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="DisplayUrl" />
                  </xsl:attribute>
                  Work item
                  <xsl:if test="ChangeType[.='New']">
                    Created:
                  </xsl:if>
                  <xsl:if test="ChangeType[.='Change']">
                    Changed:
                  </xsl:if>
                  <xsl:for-each select="CoreFields/StringFields/Field">
                    <xsl:if test="ReferenceName[.='System.WorkItemType']">
                      <xsl:value-of select="NewValue"/>
                    </xsl:if>
                  </xsl:for-each>
                  <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                  <xsl:for-each select="CoreFields/IntegerFields/Field">
                    <xsl:if test="ReferenceName[.='System.Id']">
                      <xsl:value-of select="NewValue"/>
                    </xsl:if>
                  </xsl:for-each>
                  -
                  <xsl:value-of select="WorkItemTitle" />
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                Work item
                <xsl:if test="ChangeType[.='New']">
                  Created:
                </xsl:if>
                <xsl:if test="ChangeType[.='Change']">
                  Changed:
                </xsl:if>
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.WorkItemType']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
                <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                <xsl:for-each select="CoreFields/IntegerFields/Field">
                  <xsl:if test="ReferenceName[.='System.Id']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
                -
                <xsl:value-of select="WorkItemTitle" />
              </xsl:otherwise>
            </xsl:choose>            
        </b>
          </div>
        <br />
        <table>
          <tr>
            <td>
                Team project:
            </td>
            <td class="PropValue">
                <xsl:value-of select="PortfolioProject" />
            </td>
          </tr>
          <tr>
            <td>
                Area:
            </td>
            <td class="PropValue">
                <xsl:value-of select="AreaPath" />
            </td>
          </tr>
          <tr>
            <td>
                Iteration:
            </td>
            <td class="PropValue">
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.IterationPath']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td>
                Assigned to:
            </td>
            <td class="PropValue">
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.AssignedTo']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td>
                State:
            </td>
            <td class="PropValue">
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.State']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td>
                Reason:
            </td>
            <td class="PropValue">
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.Reason']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td>
                Changed by:
            </td>
            <td class="PropValue">
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.ChangedBy']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td>
                Changed date:
            </td>
            <td class="PropValue">
                <xsl:for-each select="CoreFields/StringFields/Field">
                  <xsl:if test="ReferenceName[.='System.ChangedDate']">
                    <xsl:value-of select="NewValue"/>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
        </table>

        <br/>

        <xsl:if test="boolean(/WorkItemChangedEvent/ChangedFields/IntegerFields/Field) or boolean(/WorkItemChangedEvent/ChangedFields/StringFields/Field) or boolean(/WorkItemChangedEvent/TextFields/TextField)">
            <xsl:if test="ChangeType[.='New']">
              Other fields
            </xsl:if>
            <xsl:if test="ChangeType[.='Change']">
              Changed fields
            </xsl:if>
        </xsl:if>

        <xsl:if test="ChangeType[.='Change']">

          <table class="WithBorder">
            <xsl:if test="boolean(/WorkItemChangedEvent/TextFields/TextField)">
              <tr>
                <td class="ColHeadingMedium">
                    Field
                </td>
                <td class="ColHeading">
                    New Value
                </td>
              </tr>
            </xsl:if>

            <xsl:for-each select="TextFields/TextField">
              <tr>
                <td class="Col1Data">
                    <xsl:value-of select="Name"/>
                </td>
                <xsl:choose>
                  <xsl:when test="Value[.='']">
                    <td class="ColData">
                      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="ColData">
                        <xsl:value-of disable-output-escaping="yes" select="Value"/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
              </tr>
            </xsl:for-each>
          </table>

          <xsl:if test="boolean(/WorkItemChangedEvent/TextFields/TextField)">
            <br/>
          </xsl:if>

          <xsl:if test="boolean(/WorkItemChangedEvent/ChangedFields/IntegerFields/Field) or boolean(/WorkItemChangedEvent/ChangedFields/StringFields/Field)">
          <table class="WithBorder">
              <xsl:if test="boolean(/WorkItemChangedEvent/ChangedFields/IntegerFields/Field) or boolean(/WorkItemChangedEvent/ChangedFields/StringFields/Field)">
              <tr>
                <td class="ColHeadingMedium">
                    Field
                </td>
                <td class="ColHeading">
                    New Value
                </td>
                <td class="ColHeading">
                    Old Value
                </td>
              </tr>
              </xsl:if>

            <xsl:for-each select="ChangedFields/IntegerFields/Field">
              <tr>
                <td class="Col1Data">
                    <xsl:value-of select="Name"/>
                </td>
                <xsl:choose>
                  <xsl:when test="NewValue[.='']">
                    <td class="ColData">
                      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="ColData">
                        <xsl:value-of select="NewValue"/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:choose>
                  <xsl:when test="OldValue[.='']">
                    <td class="ColData">
                      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="ColData">
                        <xsl:value-of select="OldValue"/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
              </tr>
            </xsl:for-each>
            <xsl:for-each select="ChangedFields/StringFields/Field">
                <xsl:if test="ReferenceName[.!='System.ChangedBy']">
                    <tr>
                        <td class="Col1Data">
                            <xsl:value-of select="Name"/>
                        </td>
                        <xsl:choose>
                            <xsl:when test="NewValue[.='']">
                                <td class="ColData">
                                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                                </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class="ColData">
                                    <xsl:value-of select="NewValue"/>
                                </td>
                            </xsl:otherwise>
                        </xsl:choose>
                        <xsl:choose>
                            <xsl:when test="OldValue[.='']">
                                <td class="ColData">
                                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                                </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class="ColData">
                                    <xsl:value-of select="OldValue"/>
                                </td>
                            </xsl:otherwise>
                        </xsl:choose>
                    </tr>
                </xsl:if>
            </xsl:for-each>
          </table>
      </xsl:if> 
      </xsl:if> <!-- changetype = change -->

        <xsl:if test="ChangeType[.='New']">
          <table class="WithBorder">
            
            <xsl:if test="boolean(/WorkItemChangedEvent/TextFields/TextField) or boolean(/WorkItemChangedEvent/ChangedFields/IntegerFields/Field) or boolean(/WorkItemChangedEvent/ChangedFields/StringFields/Field)">
              <tr>
                <td class="ColHeadingMedium">
                    Field
                </td>
                <td class="ColHeading">
                    New Value
                </td>
              </tr>
            </xsl:if>

            <xsl:for-each select="TextFields/TextField">
              <tr>
                <td class="Col1Data">
                    <xsl:value-of select="Name"/>
                </td>
                <xsl:choose>
                  <xsl:when test="Value[.='']">
                    <td class="ColData">
                      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="ColData">
                        <xsl:value-of disable-output-escaping="yes" select="Value"/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
              </tr>
            </xsl:for-each>

            <xsl:for-each select="ChangedFields/IntegerFields/Field">
              <tr>
                <td class="Col1Data">
                    <xsl:value-of select="Name"/>
                </td>
                <xsl:choose>
                  <xsl:when test="NewValue[.='']">
                    <td class="ColData">
                      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td class="ColData">
                        <xsl:value-of select="NewValue"/>
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
              </tr>
            </xsl:for-each>
            <xsl:for-each select="ChangedFields/StringFields/Field">
                <xsl:if test="ReferenceName[.!='System.ChangedBy']">
                    <tr>
                        <td class="Col1Data">
                            <xsl:value-of select="Name"/>
                        </td>
                        <xsl:choose>
                            <xsl:when test="NewValue[.='']">
                                <td class="ColData">
                                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                                </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class="ColData">
                                    <xsl:value-of select="NewValue"/>
                                </td>
                            </xsl:otherwise>
                        </xsl:choose>
                    </tr>
                </xsl:if>
            </xsl:for-each>
          </table>
        </xsl:if>

        <xsl:if test="boolean(/WorkItemChangedEvent/AddedFiles) or boolean(/WorkItemChangedEvent/AddedResourceLinks) or boolean(/WorkItemChangedEvent/AddedRelations)">
          <br/>
          Links and Attachments
          <table class="WithBorder">
            <tr>
              <td class="ColHeadingMedium">
                Type
              </td>
              <td class="ColHeading">
                Description
              </td>              
            </tr>

            <xsl:for-each select="AddedFiles/AddedFile">
              <tr>
                <td class="Col1Data">
                  File Attachment
                </td>
                <td class="ColData">
                  <xsl:value-of select="Name"/>
                </td>                
              </tr>
            </xsl:for-each>

            <xsl:for-each select="AddedResourceLinks/AddedResourceLink">
              <tr>
                <td class="Col1Data">
                  Link
                </td>
                <td class="ColData">
                  <xsl:value-of select="Resource"/>
                </td>                
              </tr>
            </xsl:for-each>

            <xsl:for-each select="AddedRelations/AddedRelation">
              <tr>
                <td class="Col1Data">
                  Related Work Item
                </td>
                <td class="ColData">
                  <xsl:value-of select="WorkItemId"/>
                </td>                
              </tr>
            </xsl:for-each>
            
          </table>
        </xsl:if>       

        <xsl:if test="boolean(/WorkItemChangedEvent/DeletedFiles)">
          <br/>
          1 or more attachments have been deleted.  See work item for details.
        </xsl:if>
        
        <xsl:if test="boolean(/WorkItemChangedEvent/DeletedResourceLinks)">
          <br/>
          1 or more links have been deleted.  See work item for details.
        </xsl:if>

        <xsl:if test="boolean(/WorkItemChangedEvent/ChangedResourceLinks)">
          <br/>
          1 or more links have been changed.  See work item for details.
        </xsl:if>

        <xsl:if test="boolean(/WorkItemChangedEvent/DeletedRelations)">
          <br/>
          1 or more related work items have been deleted.  See work item for details.
        </xsl:if>

        <xsl:if test="boolean(/WorkItemChangedEvent/ChangedRelations)">
          <br/>
          1 or more related work items have been changed.  See work item for details.
        </xsl:if>

        <xsl:call-template name="footer">
      <xsl:with-param name="format" select="'html'"/>
      <xsl:with-param name="alertOwner" select="Subscriber"/>
      <xsl:with-param name="timeZoneOffset" select="TimeZoneOffset"/>
      <xsl:with-param name="timeZoneName" select="TimeZone"/>
      </xsl:call-template>
      </body>
  </xsl:template>
</xsl:stylesheet>
