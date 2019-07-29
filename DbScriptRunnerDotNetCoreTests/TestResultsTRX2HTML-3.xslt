<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:tt="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <xsl:output method="html" indent="yes" />
  <xsl:template match="/tt:TestRun/tt:Results">
    <html>
		<head><title>Test Results</title></head>
		<body>
		  <table>
			<xsl:for-each select="tt:UnitTestResult">
			  <tr>
				  <td><xsl:value-of select="@testName" /></td>
				  <td><xsl:value-of select="@outcome" /></td>
				  <td><xsl:value-of select="@duration" /></td>
			  </tr>
			</xsl:for-each>
		  </table>
		</body>
    </html>
  </xsl:template>
</xsl:stylesheet>