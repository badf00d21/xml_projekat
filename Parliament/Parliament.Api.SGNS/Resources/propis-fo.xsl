<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:parliament="http://www.xmlProjekat.com/akt"
    xmlns:fo="http://www.w3.org/1999/XSL/Format" version="2.0">
    <xsl:template match="/">
        <fo:root>
            <fo:layout-master-set>
                <fo:simple-page-master master-name="propis-strana">
                    <fo:region-body margin="1in"/>
                </fo:simple-page-master>
            </fo:layout-master-set>
            <fo:page-sequence master-reference="propis-strana">
                <fo:flow flow-name="xsl-region-body">
                    <fo:block font-family="Arial" font-size="10px"   width="70%" margin-left="10%" margin-right="10%" >
                        <xsl:for-each select="p:Preambula">
                        </xsl:for-each>
                        <xsl:for-each select="p:Prilog">   
                        </xsl:for-each>
                        <xsl:choose>
                            <xsl:when test="p:Deo">
                                <xsl:for-each select="//p:Deo">
                                    <fo:block font-family="Arial" text-align="center" font-size="23" margin="20px" >
                                        <xsl:value-of select="NazivDela"/>
                                    </fo:block>
                                    <xsl:for-each select="p:Glava">
                                        <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                                            <xsl:value-of select="NazivGlave"/>
                                        </fo:block>
                                        <xsl:for-each select="p:Odeljak">
                                            <fo:block font-family="Arial"  text-align="center" font-size="17" margin="20px">
                                                <xsl:value-of select="NazivOdeljka"/>
                                            </fo:block>
                                            <xsl:choose>
                                                <xsl:when test="p:Pododeljak">
                                                    <xsl:for-each select="p:Pododeljak">
                                                        <fo:block font-family="Arial" text-align="center" font-size="15" margin="20px">
                                                            <xsl:value-of select="NazivPododeljka"/>
                                                        </fo:block>
                                                        <xsl:for-each select="p:Clan">
                                                            <fo:block font-family="Arial" text-align="center" font-size="14" margin="20px">
                                                                <xsl:value-of select="NazivClana"/>
                                                            </fo:block>
                                                            <fo:block font-family="Arial" font-size="8" >
                                                                <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                            </fo:block>
                                                            <xsl:for-each select="p:Stav">
                                                                <fo:block font-family="Arial" font-size="8" >
                                                                    <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                </fo:block>
                                                                <xsl:for-each select="p:Tacka">
                                                                    <fo:block font-family="Arial" font-size="8" >
                                                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                    </fo:block>
                                                                    <xsl:for-each select="p:Podtacka">
                                                                        <fo:block font-family="Arial" font-size="8" >
                                                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                        </fo:block>
                                                                        <xsl:for-each select="p:Alineja">
                                                                            <fo:block font-family="Arial" font-size="8" >
                                                                                <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                            </fo:block>
                                                                        </xsl:for-each>
                                                                    </xsl:for-each>
                                                                </xsl:for-each>
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                    <xsl:for-each select="p:Clan">
                                                        <fo:block font-family="Arial" text-align="center" font-size="14" margin="20px">
                                                            <xsl:value-of select="NazivClana"/>
                                                        </fo:block>
                                                        <fo:block font-family="Arial" font-size="8" >
                                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                        </fo:block>
                                                        <xsl:for-each select="p:Stav">
                                                            <fo:block font-family="Arial" font-size="8" >
                                                                <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                            </fo:block>
                                                            <xsl:for-each select="p:Tacka">
                                                                <fo:block font-family="Arial" font-size="8" >
                                                                    <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                </fo:block>
                                                                <xsl:for-each select="p:Podtacka">
                                                                    <fo:block font-family="Arial" font-size="8" >
                                                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                    </fo:block>
                                                                    <xsl:for-each select="p:Alineja">
                                                                        <fo:block font-family="Arial" font-size="8" >
                                                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                        </fo:block>
                                                                    </xsl:for-each>
                                                                </xsl:for-each>
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                                                </xsl:otherwise>
                                            </xsl:choose>
                                        </xsl:for-each>
                                    </xsl:for-each>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:when test="p:Glava">
                                <xsl:for-each select="p:Glava">
                                    <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                                        <xsl:value-of select="NazivGlave"/>
                                    </fo:block>
                                    <xsl:for-each select="p:Odeljak">
                                        <fo:block font-family="Arial"  text-align="center" font-size="17" margin="20px">
                                            <xsl:value-of select="NazivOdeljka"/>
                                        </fo:block>
                                        <xsl:choose>
                                            <xsl:when test="p:Pododeljak">
                                                <xsl:for-each select="p:Pododeljak">
                                                    <fo:block font-family="Arial" text-align="center" font-size="15" margin="20px">
                                                        <xsl:value-of select="NazivPododeljka"/>
                                                    </fo:block>
                                                    <xsl:for-each select="p:Clan">
                                                        <fo:block font-family="Arial" text-align="center" font-size="14" margin="20px">
                                                            <xsl:value-of select="NazivClana"/>
                                                        </fo:block>
                                                        <fo:block font-family="Arial" font-size="8" >
                                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                        </fo:block>
                                                        <xsl:for-each select="p:Stav">
                                                            <fo:block font-family="Arial" font-size="8" >
                                                                <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                            </fo:block>
                                                            <xsl:for-each select="p:Tacka">
                                                                <fo:block font-family="Arial" font-size="8" >
                                                                    <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                </fo:block>
                                                                <xsl:for-each select="p:Podtacka">
                                                                    <fo:block font-family="Arial" font-size="8" >
                                                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                    </fo:block>
                                                                    <xsl:for-each select="p:Alineja">
                                                                        <fo:block font-family="Arial" font-size="8" >
                                                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                        </fo:block>
                                                                    </xsl:for-each>
                                                                </xsl:for-each>
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                                                </xsl:for-each>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:for-each select="p:Clan">
                                                    <fo:block font-family="Arial" text-align="center" font-size="14" margin="20px">
                                                        <xsl:value-of select="NazivClana"/>
                                                    </fo:block>
                                                    <fo:block font-family="Arial" font-size="8" >
                                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                    </fo:block>
                                                    <xsl:for-each select="p:Stav">
                                                        <fo:block font-family="Arial" font-size="8" >
                                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                        </fo:block>
                                                        <xsl:for-each select="p:Tacka">
                                                            <fo:block font-family="Arial" font-size="8" >
                                                                <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                            </fo:block>
                                                            <xsl:for-each select="p:Podtacka">
                                                                <fo:block font-family="Arial" font-size="8" >
                                                                    <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                </fo:block>
                                                                <xsl:for-each select="p:Alineja">
                                                                    <fo:block font-family="Arial" font-size="8" >
                                                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                                    </fo:block>
                                                                </xsl:for-each>
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                                                </xsl:for-each>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </xsl:for-each>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:for-each select="p:Clan">
                                    <fo:block font-family="Arial" text-align="center" font-size="14" margin="20px">
                                        <xsl:value-of select="NazivClana"/>
                                    </fo:block>
                                    <fo:block font-family="Arial" font-size="8" >
                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                    </fo:block>
                                    <xsl:for-each select="p:Stav">
                                        <fo:block font-family="Arial" font-size="8" >
                                            <xsl:value-of select="p:TekstualniSadrzaj"/>
                                        </fo:block>
                                        <xsl:for-each select="p:Tacka">
                                            <fo:block font-family="Arial" font-size="8" >
                                                <xsl:value-of select="p:TekstualniSadrzaj"/>
                                            </fo:block>
                                            <xsl:for-each select="p:Podtacka">
                                                <fo:block font-family="Arial" font-size="8" >
                                                    <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                </fo:block>
                                                <xsl:for-each select="p:Alineja">
                                                    <fo:block font-family="Arial" font-size="8" >
                                                        <xsl:value-of select="p:TekstualniSadrzaj"/>
                                                    </fo:block>
                                                </xsl:for-each>
                                            </xsl:for-each>
                                        </xsl:for-each>
                                    </xsl:for-each>
                                </xsl:for-each>
                            </xsl:otherwise>
                        </xsl:choose>
                    </fo:block>
                </fo:flow>
            </fo:page-sequence>
        </fo:root>
    </xsl:template>
</xsl:stylesheet>