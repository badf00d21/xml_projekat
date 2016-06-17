<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:parliament="http://www.parliament.rs/schema" exclude-result-prefixes="xs"
    xmlns:fox="http://xmlgraphics.apache.org/fop/extensions"
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

          <fo:block font-family="Arial" font-size="10px" width="70%" margin-left="10%" margin-right="10%">

            <fo:block font-family="Arial" text-align="center" font-size="23" margin="20px">
              <xsl:value-of select="//parliament:NazivPropisa"/>
            </fo:block>

            <xsl:for-each select="//parliament:Preambula">

              <xsl:for-each select="//parliament:PravniOsnov">
                <fo:block font-family="Arial" text-align="center" font-size="14"
                                                  margin="20px">
                  Pravni osnov:
                  <xsl:value-of select="current()"/>
                </fo:block>

              </xsl:for-each>
              <xsl:for-each select="//parliament:Preambula/parliament:NadlezniOrgan">

                <fo:block font-family="Arial" text-align="center" font-size="14"
                                                  margin="20px">
                  Ime nadleznog organa:
                  <xsl:value-of select="//parliament:Ime"/>
                </fo:block>
                <fo:block font-family="Arial" text-align="center" font-size="14"
                                                  margin="20px">
                  Prezime nadleznog organa:
                  <xsl:value-of select="//parliament:Prezime"/>
                </fo:block>
                <fo:block font-family="Arial" text-align="center" font-size="14"
                                                  margin="20px">
                  Email nadleznog organa:
                  <xsl:value-of select="//parliament:Email"/>
                </fo:block>
              </xsl:for-each>
              <xsl:for-each select="//parliament:Saglasnost">
                <br/>
                <xsl:for-each select="//parliament:Naznaka">
                  <fo:block font-family="Arial" text-align="center" font-size="14"
                                                margin="20px">
                    Naznaka saglasnosti:
                    <xsl:value-of select="current()"/>
                  </fo:block>
                </xsl:for-each>
                <xsl:for-each select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan">
                  <!-- Ovde sad kazemo da hocemo NadlezniOrgan iz taga Saglasnost -->
                  <fo:block font-family="Arial" text-align="center" font-size="14"
                                                margin="20px">
                    Ime nadleznog organa:
                    <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Ime"/>
                  </fo:block>
                  <fo:block font-family="Arial" text-align="center" font-size="14"
                                                margin="20px">
                    Prezime nadleznog organa:
                    <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Prezime"/>
                  </fo:block>
                  <fo:block font-family="Arial" text-align="center" font-size="14"
                                                margin="20px">
                    Email nadleznog organa:
                    <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Email"/>
                  </fo:block>
                </xsl:for-each>
              </xsl:for-each>
            </xsl:for-each>

            <xsl:choose>
              <xsl:when test="//parliament:Deo">
                <xsl:for-each select="//parliament:Deo">
                  <fo:block font-family="Arial" text-align="center" font-size="21" margin="20px">
                    <xsl:value-of select="//parliament:NazivDela"/>
                  </fo:block>
                  <xsl:for-each select="//parliament:Glava">
                    <fo:block id="{@Id}" font-family="Arial" text-align="center" font-size="19" margin="20px">
                      <xsl:value-of select="//parliament:NazivGlave"/>
                    </fo:block>
                    <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                      Glava
                      <xsl:value-of select="//parliament:Deo/parliament:Glava/@parliament:RedniBroj"/>
                    </fo:block>
                    <xsl:for-each select="//parliament:Odeljak">
                      <fo:block font-family="Arial" text-align="center" font-size="17" margin="20px">
                        <xsl:value-of select="//parliament:NazivOdeljka"/>
                      </fo:block>
                      <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                        Odeljak
                        <xsl:value-of select="//@parliament:RedniBroj"/>
                      </fo:block>
                      <xsl:choose>
                        <xsl:when test="//parliament:Pododeljak">
                          <fo:block font-family="Arial" text-align="center" font-size="15" margin="20px">
                            <xsl:value-of select="//parliament:NazivPododeljka"/>
                          </fo:block>
                          <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                            Pododeljak
                            <xsl:value-of select="//@parliament:RedniBroj"/>
                          </fo:block>
                          <xsl:for-each select="//parliament:Clan">
                            <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                              <xsl:value-of select="//parliament:NazivClana"/>
                            </fo:block>
                            <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                              Član
                              <xsl:value-of select="//@parliament:RedniBroj"/>
                            </fo:block>
                            <xsl:choose>
                              <xsl:when test="//parliament:TekstualniSadrzaj">

                                <xsl:value-of select="current()"/>

                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:for-each select="//parliament:Stav">
                                  <xsl:choose>
                                    <xsl:when test="//parliament:TekstualniSadrzaj">

                                      <xsl:value-of select="current()"/>

                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:for-each select="//parliament:Tacka">
                                        <xsl:choose>
                                          <xsl:when test="//parliament:TekstualniSadrzaj">

                                            <xsl:value-of select="current()"/>

                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:for-each select="//parliament:Podtacka">
                                              <xsl:choose>
                                                <xsl:when test="//parliament:TekstualniSadrzaj">

                                                  <xsl:value-of select="current()"/>

                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <xsl:for-each select="//parliament:Alineja">
                                                    Sadržaj alineje:
                                                    <xsl:value-of select="current()"/>

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
                            <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                              <xsl:value-of select="//parliament:NazivClana"/>
                            </fo:block>
                            <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                              Član
                              <xsl:value-of select="//@parliament:RedniBroj"/>
                            </fo:block>
                            <xsl:choose>
                              <xsl:when test="//parliament:TekstualniSadrzaj">

                                <xsl:value-of select="current()"/>

                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:for-each select="//parliament:Stav">
                                  <xsl:choose>
                                    <xsl:when test="//parliament:TekstualniSadrzaj">

                                      <xsl:value-of select="current()"/>

                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:for-each select="//parliament:Tacka">
                                        <xsl:choose>
                                          <xsl:when test="//parliament:TekstualniSadrzaj">

                                            <xsl:value-of select="current()"/>

                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:for-each select="//parliament:Podtacka">
                                              <xsl:choose>
                                                <xsl:when test="//parliament:TekstualniSadrzaj">

                                                  <xsl:value-of select="current()"/>

                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <xsl:for-each select="//parliament:Alineja">
                                                    Sadržaj alineje:
                                                    <xsl:value-of select="current()"/>

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
                  <fo:block id="{@Id}" font-family="Arial" text-align="center" font-size="19" margin="20px">
                    <xsl:value-of select="//parliament:NazivGlave"/>
                  </fo:block>
                  <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                    Glava
                    <xsl:value-of select="//@parliament:RedniBroj"/>
                  </fo:block>
                  <xsl:for-each select="//parliament:Odeljak">
                    <fo:block font-family="Arial" text-align="center" font-size="17" margin="20px">
                      <xsl:value-of select="//parliament:NazivOdeljka"/>
                    </fo:block>
                    <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                      Odeljak
                      <xsl:value-of select="//@parliament:RedniBroj"/>
                    </fo:block>
                    <xsl:choose>
                      <xsl:when test="//parliament:Pododeljak">
                        <fo:block font-family="Arial" text-align="center" font-size="15" margin="20px">
                          <xsl:value-of select="//parliament:NazivPododeljka"/>
                        </fo:block>
                        <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                          Pododeljak
                          <xsl:value-of select="//@parliament:RedniBroj"/>
                        </fo:block>
                        <xsl:for-each select="//parliament:Clan">
                          <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                            <xsl:value-of select="//parliament:NazivClana"/>
                          </fo:block>
                          <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                            Član
                            <xsl:value-of select="//@parliament:RedniBroj"/>
                          </fo:block>
                          <xsl:choose>
                            <xsl:when test="//parliament:TekstualniSadrzaj">

                              <xsl:value-of select="current()"/>

                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:for-each select="//parliament:Stav">
                                <xsl:choose>
                                  <xsl:when test="//parliament:TekstualniSadrzaj">

                                    <xsl:value-of select="current()"/>

                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:for-each select="//parliament:Tacka">
                                      <xsl:choose>
                                        <xsl:when test="//parliament:TekstualniSadrzaj">

                                          <xsl:value-of select="current()"/>

                                        </xsl:when>
                                        <xsl:otherwise>
                                          <xsl:for-each select="//parliament:Podtacka">
                                            <xsl:choose>
                                              <xsl:when test="//parliament:TekstualniSadrzaj">

                                                <xsl:value-of select="current()"/>

                                              </xsl:when>
                                              <xsl:otherwise>
                                                <xsl:for-each select="//parliament:Alineja">
                                                  Sadržaj alineje:
                                                  <xsl:value-of select="current()"/>

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
                          <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                            <xsl:value-of select="//parliament:NazivClana"/>
                          </fo:block>
                          <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                            Član
                            <xsl:value-of select="//@parliament:RedniBroj"/>
                          </fo:block>
                          <xsl:choose>
                            <xsl:when test="//parliament:TekstualniSadrzaj">

                              <xsl:value-of select="current()"/>

                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:for-each select="//parliament:Stav">
                                <xsl:choose>
                                  <xsl:when test="//parliament:TekstualniSadrzaj">

                                    <xsl:value-of select="current()"/>

                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:for-each select="//parliament:Tacka">
                                      <xsl:choose>
                                        <xsl:when test="//parliament:TekstualniSadrzaj">

                                          <xsl:value-of select="current()"/>

                                        </xsl:when>
                                        <xsl:otherwise>
                                          <xsl:for-each select="//parliament:Podtacka">
                                            <xsl:choose>
                                              <xsl:when test="//parliament:TekstualniSadrzaj">

                                                <xsl:value-of select="current()"/>

                                              </xsl:when>
                                              <xsl:otherwise>
                                                <xsl:for-each select="//parliament:Alineja">
                                                  Sadržaj alineje:
                                                  <xsl:value-of select="current()"/>

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
                  <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                    <xsl:value-of select="//parliament:NazivClana"/>
                  </fo:block>
                  <fo:block font-family="Arial" text-align="center" font-size="19" margin="20px">
                    Član
                    <xsl:value-of select="//@parliament:RedniBroj"/>
                  </fo:block>
                  <xsl:choose>
                    <xsl:when test="//parliament:TekstualniSadrzaj">

                      <xsl:value-of select="current()"/>

                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:for-each select="//parliament:Stav">
                        <xsl:choose>
                          <xsl:when test="//parliament:TekstualniSadrzaj">

                            <xsl:value-of select="current()"/>

                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:for-each select="//parliament:Tacka">
                              <xsl:choose>
                                <xsl:when test="//parliament:TekstualniSadrzaj">

                                  <xsl:value-of select="current()"/>

                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:for-each select="//parliament:Podtacka">
                                    <xsl:choose>
                                      <xsl:when test="//parliament:TekstualniSadrzaj">

                                        <xsl:value-of select="current()"/>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        <xsl:for-each select="//parliament:Alineja">
                                          Sadržaj alineje:
                                          <xsl:value-of select="current()"/>

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
              <fo:block id="{@Id}" font-family="Arial" text-align="center" font-size="19" margin="20px">
                Naziv Priloga:
                <xsl:value-of select="current()"/>
              </fo:block>
            </xsl:for-each>


          </fo:block>
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>
</xsl:stylesheet>