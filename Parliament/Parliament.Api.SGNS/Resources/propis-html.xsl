<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:parliament="http://www.parliament.rs/schema" exclude-result-prefixes="xs" version="2.0">
    <xsl:template match="/">

        <html>
            <head>
              <style type="text/css">
                  body {
                    background-color:white;
                    font-family: "Times New Roman", Times, serif;
                  
                  }
                  div.container {
                    width: 70%;
                    margin-left: auto;
                    margin-right: auto;
                    background-color:white;
                    border-color:white;
                    border-style: solid;
                    border-width: 1px;
                  }
                  
                  h1, h2, h3, h4, h5, h6 {
                    text-align: center;
                    
                  }
                  
                  p{
                    text-align:justify;
                    width:60%;
                    margin-left:auto;
                    margin-right:auto;
                    text-indent: 50px;
                    font-size:14px;
                  }
                  
                  h1 {
                    font-size:50px;
                  }
                  h2{
                    font-size:40px;
                  }
                  h3{
                    font-size:35px;
                  }
                  h4{
                    font-size:30px;
                  }
                  h5{
                    font-size:25px;
                  }
                  h6{
                    font-size:20px;
                  }

              </style>        

            </head>
            <body>
                <div class="container">
                    <h1>
                        <xsl:value-of select="//parliament:NazivPropisa"/>
                    </h1>
                 
                    <xsl:for-each select="//parliament:Preambula">
                     
                        <xsl:for-each select="//parliament:PravniOsnov">
                            <p> Pravni osnov: 
                                <xsl:value-of select="current()"/>
                            </p>
                          
                        </xsl:for-each>
                        <xsl:for-each select="//parliament:Preambula/parliament:NadlezniOrgan">
                       
                            <p> Ime nadleznog organa: 
                                <xsl:value-of select="//parliament:Ime"/>
                            </p>
                            <p> Prezime nadleznog organa: 
                                <xsl:value-of select="//parliament:Prezime"/>
                            </p>
                            <p> Email nadleznog organa: 
                                <xsl:value-of select="//parliament:Email"/>
                            </p>
                        </xsl:for-each>
                        <xsl:for-each select="//parliament:Saglasnost">
                            <br/>
                            <xsl:for-each select="//parliament:Naznaka">
                                <p> Naznaka saglasnosti: 
                                    <xsl:value-of select="current()"/>
                                </p>
                            </xsl:for-each>
                            <xsl:for-each select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan">
                            
                                <p> Ime nadleznog organa: 
                                    <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Ime"/>
                                </p>
                                <p> Prezime nadleznog organa: 
                                    <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Prezime"/>
                                </p>
                                <p> Email nadleznog organa: 
                                    <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Email"/>
                                </p>
                            </xsl:for-each>
                        </xsl:for-each>
                    </xsl:for-each>
                
                    <xsl:choose>
                        <xsl:when test="//parliament:Deo">
                            <xsl:for-each select="//parliament:Deo">
                                <h2>
                                    <xsl:value-of select="//parliament:NazivDela"/>
                                </h2>
                                <xsl:for-each select="//parliament:Glava">
                                    <h3>
                                        <xsl:value-of select="//parliament:NazivGlave"/>
                                    </h3>
                                    <h6>Glava 
                                        <xsl:value-of select="//parliament:Deo/parliament:Glava/@parliament:RedniBroj"/>
                                    </h6>
                                    <xsl:for-each select="//parliament:Odeljak">
                                        <h4>
                                            <xsl:value-of select="//parliament:NazivOdeljka"/>
                                        </h4>
                                        <h6> Odeljak 
                                            <xsl:value-of select="//@parliament:RedniBroj"/>
                                        </h6>
                                        <xsl:choose>
                                            <xsl:when test="//parliament:Pododeljak">
                                                <h5>
                                                    <xsl:value-of select="//parliament:NazivPododeljka"/>
                                                </h5>
                                                <h6> Pododeljak 
                                                    <xsl:value-of select="//@parliament:RedniBroj"/>
                                                </h6>
                                                <xsl:for-each select="//parliament:Clan">
                                                    <h6>
                                                        <xsl:value-of select="//parliament:NazivClana"/>
                                                    </h6>
                                                    <h6> Član 
                                                        <xsl:value-of select="//@parliament:RedniBroj"/>
                                                    </h6>
                                                    <xsl:choose>
                                                        <xsl:when test="//parliament:TekstualniSadrzaj">
                                                            <p>
                                                                <xsl:value-of select="current()"/>
                                                            </p>
                                                        </xsl:when>
                                                        <xsl:otherwise>
                                                            <xsl:for-each select="//parliament:Stav">
                                                                <xsl:choose>
                                                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                        <p>
                                                                            <xsl:value-of select="current()"/>
                                                                        </p>
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:for-each select="//parliament:Tacka">
                                                                            <xsl:choose>
                                                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                    <p>
                                                                                        <xsl:value-of select="current()"/>
                                                                                    </p>
                                                                                </xsl:when>
                                                                                <xsl:otherwise>
                                                                                    <xsl:for-each select="//parliament:Podtacka">
                                                                                        <xsl:choose>
                                                                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                                <p>
                                                                                                    <xsl:value-of select="current()"/>
                                                                                                </p>
                                                                                            </xsl:when>
                                                                                            <xsl:otherwise>
                                                                                                <xsl:for-each select="//parliament:Alineja">
                                                                                                    <p> Sadržaj alineje: 
                                                                                                        <xsl:value-of select="current()"/>
                                                                                                    </p>
                                                                                                </xsl:for-each>
                                                                                            </xsl:otherwise>
                                                                                        </xsl:choose>
                                                                                    </xsl:for-each>
                                                                                </xsl:otherwise>
                                                                            </xsl:choose>
                                                                        </xsl:for-each>
                                                                    </xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:for-each>
                                                        </xsl:otherwise>
                                                    </xsl:choose>
                                                </xsl:for-each>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:for-each select="//parliament:Clan">
                                                    <h6>
                                                        <xsl:value-of select="//parliament:NazivClana"/>
                                                    </h6>
                                                    <h6> Član 
                                                        <xsl:value-of select="//@parliament:RedniBroj"/>
                                                    </h6>
                                                    <xsl:choose>
                                                        <xsl:when test="//parliament:TekstualniSadrzaj">
                                                            <p>
                                                                <xsl:value-of select="current()"/>
                                                            </p>
                                                        </xsl:when>
                                                        <xsl:otherwise>
                                                            <xsl:for-each select="//parliament:Stav">
                                                                <xsl:choose>
                                                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                        <p>
                                                                            <xsl:value-of select="current()"/>
                                                                        </p>
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:for-each select="//parliament:Tacka">
                                                                            <xsl:choose>
                                                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                    <p>
                                                                                        <xsl:value-of select="current()"/>
                                                                                    </p>
                                                                                </xsl:when>
                                                                                <xsl:otherwise>
                                                                                    <xsl:for-each select="//parliament:Podtacka">
                                                                                        <xsl:choose>
                                                                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                                <p>
                                                                                                    <xsl:value-of select="current()"/>
                                                                                                </p>
                                                                                            </xsl:when>
                                                                                            <xsl:otherwise>
                                                                                                <xsl:for-each select="//parliament:Alineja">
                                                                                                    <p> Sadržaj alineje: 
                                                                                                        <xsl:value-of select="current()"/>
                                                                                                    </p>
                                                                                                </xsl:for-each>
                                                                                            </xsl:otherwise>
                                                                                        </xsl:choose>
                                                                                    </xsl:for-each>
                                                                                </xsl:otherwise>
                                                                            </xsl:choose>
                                                                        </xsl:for-each>
                                                                    </xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:for-each>
                                                        </xsl:otherwise>
                                                    </xsl:choose>
                                                </xsl:for-each>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </xsl:for-each>
                                </xsl:for-each>
                            </xsl:for-each>
                        </xsl:when>
                        <xsl:when test="//parliament:Glava">
                            <xsl:for-each select="//parliament:Glava">
                                <h3>
                                    <xsl:value-of select="//parliament:NazivGlave"/>
                                </h3>
                                <h6>Glava 
                                    <xsl:value-of select="//@parliament:RedniBroj"/>
                                </h6>
                                <xsl:for-each select="//parliament:Odeljak">
                                    <h4>
                                        <xsl:value-of select="//parliament:NazivOdeljka"/>
                                    </h4>
                                    <h6> Odeljak 
                                        <xsl:value-of select="//@parliament:RedniBroj"/>
                                    </h6>
                                    <xsl:choose>
                                        <xsl:when test="//parliament:Pododeljak">
                                            <h5>
                                                <xsl:value-of select="//parliament:NazivPododeljka"/>
                                            </h5>
                                            <h6> Pododeljak 
                                                <xsl:value-of select="//@parliament:RedniBroj"/>
                                            </h6>
                                            <xsl:for-each select="//parliament:Clan">
                                                <h6>
                                                    <xsl:value-of select="//parliament:NazivClana"/>
                                                </h6>
                                                <h6> Član 
                                                    <xsl:value-of select="//@parliament:RedniBroj"/>
                                                </h6>
                                                <xsl:choose>
                                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                                        <p>
                                                            <xsl:value-of select="current()"/>
                                                        </p>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:for-each select="//parliament:Stav">
                                                            <xsl:choose>
                                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                    <p>
                                                                        <xsl:value-of select="current()"/>
                                                                    </p>
                                                                </xsl:when>
                                                                <xsl:otherwise>
                                                                    <xsl:for-each select="//parliament:Tacka">
                                                                        <xsl:choose>
                                                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                <p>
                                                                                    <xsl:value-of select="current()"/>
                                                                                </p>
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:for-each select="//parliament:Podtacka">
                                                                                    <xsl:choose>
                                                                                        <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                            <p>
                                                                                                <xsl:value-of select="current()"/>
                                                                                            </p>
                                                                                        </xsl:when>
                                                                                        <xsl:otherwise>
                                                                                            <xsl:for-each select="//parliament:Alineja">
                                                                                                <p> Sadržaj alineje: 
                                                                                                    <xsl:value-of select="current()"/>
                                                                                                </p>
                                                                                            </xsl:for-each>
                                                                                        </xsl:otherwise>
                                                                                    </xsl:choose>
                                                                                </xsl:for-each>
                                                                            </xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:for-each>
                                                                </xsl:otherwise>
                                                            </xsl:choose>
                                                        </xsl:for-each>
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:for-each>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:for-each select="//parliament:Clan">
                                                <h6>
                                                    <xsl:value-of select="//parliament:NazivClana"/>
                                                </h6>
                                                <h6> Član 
                                                    <xsl:value-of select="//@parliament:RedniBroj"/>
                                                </h6>
                                                <xsl:choose>
                                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                                        <p>
                                                            <xsl:value-of select="current()"/>
                                                        </p>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:for-each select="//parliament:Stav">
                                                            <xsl:choose>
                                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                    <p>
                                                                        <xsl:value-of select="current()"/>
                                                                    </p>
                                                                </xsl:when>
                                                                <xsl:otherwise>
                                                                    <xsl:for-each select="//parliament:Tacka">
                                                                        <xsl:choose>
                                                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                <p>
                                                                                    <xsl:value-of select="current()"/>
                                                                                </p>
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:for-each select="//parliament:Podtacka">
                                                                                    <xsl:choose>
                                                                                        <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                                            <p>
                                                                                                <xsl:value-of select="current()"/>
                                                                                            </p>
                                                                                        </xsl:when>
                                                                                        <xsl:otherwise>
                                                                                            <xsl:for-each select="//parliament:Alineja">
                                                                                                <p> Sadržaj alineje: 
                                                                                                    <xsl:value-of select="current()"/>
                                                                                                </p>
                                                                                            </xsl:for-each>
                                                                                        </xsl:otherwise>
                                                                                    </xsl:choose>
                                                                                </xsl:for-each>
                                                                            </xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:for-each>
                                                                </xsl:otherwise>
                                                            </xsl:choose>
                                                        </xsl:for-each>
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:for-each>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:for-each>
                            </xsl:for-each>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:for-each select="//parliament:Clan">
                                <h6>
                                    <xsl:value-of select="//parliament:NazivClana"/>
                                </h6>
                                <h6> Član 
                                    <xsl:value-of select="//@parliament:RedniBroj"/>
                                </h6>
                                <xsl:choose>
                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                        <p>
                                            <xsl:value-of select="current()"/>
                                        </p>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:for-each select="//parliament:Stav">
                                            <xsl:choose>
                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                    <p>
                                                        <xsl:value-of select="current()"/>
                                                    </p>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                    <xsl:for-each select="//parliament:Tacka">
                                                        <xsl:choose>
                                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                <p>
                                                                    <xsl:value-of select="current()"/>
                                                                </p>
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:for-each select="//parliament:Podtacka">
                                                                    <xsl:choose>
                                                                        <xsl:when test="//parliament:TekstualniSadrzaj">
                                                                            <p>
                                                                                <xsl:value-of select="current()"/>
                                                                            </p>
                                                                        </xsl:when>
                                                                        <xsl:otherwise>
                                                                            <xsl:for-each select="//parliament:Alineja">
                                                                                <p> Sadržaj alineje: 
                                                                                    <xsl:value-of select="current()"/>
                                                                                </p>
                                                                            </xsl:for-each>
                                                                        </xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:for-each>
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </xsl:for-each>
                                                </xsl:otherwise>
                                            </xsl:choose>
                                        </xsl:for-each>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:for-each>
                        </xsl:otherwise>
                    </xsl:choose>
                    <xsl:for-each select="//parliament:Prilog">
                        <h3> Naziv Priloga: 
                            <xsl:value-of select="current()"/>
                        </h3>
                    </xsl:for-each>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>