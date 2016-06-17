<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:parliament="http://www.parliament.rs/schema"
    exclude-result-prefixes="xs"
    version="2.0">
    <xsl:template match="/">
        
        <html>
            <head>
                <title> Propis - <xsl:value-of select="/*/NazivPropisa"/></title>
            </head>
            <body>
                <div class="container">
                    <h1><xsl:value-of select="/*/NazivPropisa"/></h1>
                    <xsl:for-each select="//propis:Preambula">
                    </xsl:for-each>
                    <xsl:for-each select="//propis:Prilog">
                    </xsl:for-each>
                    </xsl:for-each>
                    <xsl:choose>
                        <xsl:when test="propis:Deo">
                            <xsl:for-each select="//propis:Deo">
                                <h2><xsl:value-of select="NazivDela"/></h2>
                                <xsl:for-each select="propis:Glava">
                                    <h3><xsl:value-of select="NazivGlave"/></h3>
                                    <xsl:for-each select="propis:Odeljak">
                                        <h4><xsl:value-of select="NazivOdeljka"/></h4>
                                        
                                        <xsl:choose>
                                            <xsl:when test="propis:Pododeljak">
                                                <xsl:for-each select="propis:Pododeljak">
                                                    <h5><xsl:value-of select="NazivPododeljka"/></h5>
                                                    
                                                    <xsl:for-each select="propis:Clan">
                                                        <h5><xsl:value-of select="NazivClana"/></h5>
                                                        
                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                        
                                                        <xsl:for-each select="propis:Stav">
                                                            
                                                            <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                            
                                                            <xsl:for-each select="propis:Tacka">
                                                                
                                                                <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                
                                                                <xsl:for-each select="propis:Podtacka">
                                                                    
                                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                    
                                                                    <xsl:for-each select="propis:Alineja">                                                            
                                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                        
                                                                    </xsl:for-each>
                                                                    
                                                                </xsl:for-each>
                                                                
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                                                </xsl:for-each>
                                            </xsl:when>
                                            
                                            <xsl:otherwise>
                                                <xsl:for-each select="propis:Clan">
                                                    <h5><xsl:value-of select="NazivClana"/></h5>
                                                    
                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                    
                                                    <xsl:for-each select="propis:Stav">
                                                        
                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                        
                                                        <xsl:for-each select="propis:Tacka">
                                                            
                                                            <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                            
                                                            <xsl:for-each select="propis:Podtacka">
                                                                
                                                                <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                
                                                                <xsl:for-each select="propis:Alineja">                                                            
                                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                    
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
                        
                        <xsl:when test="propis:Glava">
                            <xsl:for-each select="propis:Glava">
                                    <h3><xsl:value-of select="NazivGlave"/></h3>
                                    <xsl:for-each select="propis:Odeljak">
                                        <h4><xsl:value-of select="NazivOdeljka"/></h4>
                                        
                                        <xsl:choose>
                                            <xsl:when test="propis:Pododeljak">
                                                <xsl:for-each select="propis:Pododeljak">
                                                    <h5><xsl:value-of select="NazivPododeljka"/></h5>
                                                    
                                                    <xsl:for-each select="propis:Clan">
                                                        <h5><xsl:value-of select="NazivClana"/></h5>
                                                        
                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                        
                                                        <xsl:for-each select="propis:Stav">
                                                            
                                                            <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                            
                                                            <xsl:for-each select="propis:Tacka">
                                                                
                                                                <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                
                                                                <xsl:for-each select="propis:Podtacka">
                                                                    
                                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                    
                                                                    <xsl:for-each select="propis:Alineja">                                                            
                                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                        
                                                                    </xsl:for-each>
                                                                    
                                                                </xsl:for-each>
                                                                
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                                                </xsl:for-each>
                                            </xsl:when>
                                            
                                            <xsl:otherwise>
                                                <xsl:for-each select="propis:Clan">
                                                    <h5><xsl:value-of select="NazivClana"/></h5>
                                                    
                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                    
                                                    <xsl:for-each select="propis:Stav">
                                                        
                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                        
                                                        <xsl:for-each select="propis:Tacka">
                                                            
                                                            <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                            
                                                            <xsl:for-each select="propis:Podtacka">
                                                                
                                                                <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                
                                                                <xsl:for-each select="propis:Alineja">                                                            
                                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                    
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
                            <xsl:for-each select="propis:Clan">
                                                        <h5><xsl:value-of select="NazivClana"/></h5>
                                                        
                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                        
                                                        <xsl:for-each select="propis:Stav">
                                                            
                                                            <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                            
                                                            <xsl:for-each select="propis:Tacka">
                                                                
                                                                <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                
                                                                <xsl:for-each select="propis:Podtacka">
                                                                    
                                                                    <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                    
                                                                    <xsl:for-each select="propis:Alineja">                                                            
                                                                        <p><xsl:value-of select="propis:TekstualniSadrzaj"/></p>
                                                                        
                                                                    </xsl:for-each>
                                                                    
                                                                </xsl:for-each>
                                                                
                                                            </xsl:for-each>
                                                        </xsl:for-each>
                                                    </xsl:for-each>
                        </xsl:otherwise>
                    </xsl:choose>
                    
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>