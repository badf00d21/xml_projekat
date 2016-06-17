<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:parliament="http://www.parliament.rs/schema" exclude-result-prefixes="xs" version="2.0">
    <xsl:template match="/">
        <!-- Ovo Matchuje dokument i postavlja ga za trenutni kontekst, svaka sledeca putanja do taka je relativna u odnosu na ovo -->
      <html>  
        <head>
           
        </head>

        <body>
            <div class="container">
                <h1>
                    <xsl:value-of select="//parliament:NazivPropisa"/>
                </h1>
                <!-- Za naslov dokumenta ide h1 i izvlaci se vrednost naslova iz xml-a, obratite paznju na putanju (prefiks:ImeTaga/prefiks:PodTag) -->
                <xsl:for-each select="//parliament:Preambula">
                    <!-- Putanja do preambule ili ovako jer je trenutni kontekst Propis ili moze full putanja parliament:Propis/parliament:Preambula -->
                    <xsl:for-each select="//parliament:PravniOsnov">
                        <p> Pravni osnov: <xsl:value-of select="current()"/>
                        </p>
                        <!-- Ovde je u for-each selektovan PravniPropis tag pa njegova vrednost se dobija sa current(), jer nema podtagove -->
                    </xsl:for-each>
                    <xsl:for-each select="//parliament:Preambula/parliament:NadlezniOrgan">
                        <!-- Ovde je bitno staviti u putanju i preambulu jer nadlezni organ se nalazi i u tagu Saglasnost pa da ne bi bilo duplo -->
                        <p> Ime nadleznog organa: <xsl:value-of select="//parliament:Ime"/>
                        </p>
                        <p> Prezime nadleznog organa: <xsl:value-of select="//parliament:Prezime"/>
                        </p>
                        <p> Email nadleznog organa: <xsl:value-of select="//parliament:Email"/>
                        </p>
                    </xsl:for-each>
                    <xsl:for-each select="//parliament:Saglasnost">
                        <br/>
                        <xsl:for-each select="//parliament:Naznaka">
                            <p> Naznaka saglasnosti: <xsl:value-of select="current()"/>
                            </p>
                        </xsl:for-each>
                        <xsl:for-each select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan">
                            <!-- Ovde sad kazemo da hocemo NadlezniOrgan iz taga Saglasnost -->
                            <p> Ime nadleznog organa: <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Ime"/>
                            </p>
                            <p> Prezime nadleznog organa: <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Prezime"/>
                            </p>
                            <p> Email nadleznog organa: <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Email"/>
                            </p>
                        </xsl:for-each>
                    </xsl:for-each>
                </xsl:for-each>
                <!-- Moj predlog je da koristite uvek pune putanje za svaki slucaj ako niste 100% sta je trenutni kontekst itd -->
                <!-- Ono sto vam je sigurno je da je kontekst parliament:Propis, znaci kad napisete // to je umesto parliament:Propis -->
                <!-- Najpametnije je odatle ici pune putanje //parliament:Deo/parliament:Glava/parliament:Clan... -->
                <!-- Evo ispod cetiri prazna foreach-a, posto trenutno obradjujemo tag Propis, i ispod njega smo obradili Preambulu, na tom nivou mogu da se nadju jos Deo ili Glava ili Clan i opciono Prilog -->
                <xsl:choose>
                  <xsl:when test="//parliament:Deo">
                    <xsl:for-each select="//parliament:Deo">
                        <h2> <xsl:value-of select="//parliament:NazivDela"/> </h2>

                        <xsl:for-each select="//parliament:Glava">
                          <h3> <xsl:value-of select="//parliament:NazivGlave"/></h3>
                          <h6>Glava <xsl:value-of select="//parliament:Deo/parliament:Glava/@parliament:RedniBroj"/></h6>

                          <xsl:for-each select="//parliament:Odeljak">
                            <h4> <xsl:value-of select="//parliament:NazivOdeljka"/></h4>
                            <h6> Odeljak <xsl:value-of select="//@parliament:RedniBroj"/></h6>

                            <xsl:choose>
                              <xsl:when test="//parliament:Pododeljak">
                                <h5> <xsl:value-of select="//parliament:NazivPododeljka"/></h5>
                                <h6> Pododeljak <xsl:value-of select="//@parliament:RedniBroj"/> </h6>

                                <xsl:for-each select="//parliament:Clan">
                                  <h6> <xsl:value-of select="//parliament:NazivClana"/> </h6>
                                  <h6> Član <xsl:value-of select="//@parliament:RedniBroj"/> </h6>

                                  <xsl:choose>
                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                      <p>  <xsl:value-of select="current()"/> </p>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:for-each select="//parliament:Stav">
                                        <xsl:choose>
                                          <xsl:when test="//parliament:TekstualniSadrzaj">
                                            <p>  <xsl:value-of select="current()"/> </p>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:for-each select="//parliament:Tacka">
                                              <xsl:choose>
                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                  <p>  <xsl:value-of select="current()"/> </p>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <xsl:for-each select="//parliament:Podtacka">
                                                    <xsl:choose>
                                                      <xsl:when test="//parliament:TekstualniSadrzaj">
                                                        <p> <xsl:value-of select="current()"/> </p>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:for-each select="//parliament:Alineja">
                                                          <p> Sadržaj alineje: <xsl:value-of select="current()"/> </p>
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
                                  <h6> <xsl:value-of select="//parliament:NazivClana"/> </h6>
                                  <h6> Član <xsl:value-of select="//@parliament:RedniBroj"/> </h6>

                                  <xsl:choose>
                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                      <p>  <xsl:value-of select="current()"/> </p>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:for-each select="//parliament:Stav">
                                        <xsl:choose>
                                          <xsl:when test="//parliament:TekstualniSadrzaj">
                                            <p>  <xsl:value-of select="current()"/> </p>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:for-each select="//parliament:Tacka">
                                              <xsl:choose>
                                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                                  <p>  <xsl:value-of select="current()"/> </p>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <xsl:for-each select="//parliament:Podtacka">
                                                    <xsl:choose>
                                                      <xsl:when test="//parliament:TekstualniSadrzaj">
                                                        <p>  <xsl:value-of select="current()"/> </p>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:for-each select="//parliament:Alineja">
                                                          <p> Sadržaj alineje: <xsl:value-of select="current()"/> </p>
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
                      <h3> <xsl:value-of select="//parliament:NazivGlave"/></h3>
                      <h6>Glava <xsl:value-of select="//@parliament:RedniBroj"/></h6>

                      <xsl:for-each select="//parliament:Odeljak">
                        <h4> <xsl:value-of select="//parliament:NazivOdeljka"/></h4>
                        <h6> Odeljak <xsl:value-of select="//@parliament:RedniBroj"/></h6>

                        <xsl:choose>
                          <xsl:when test="//parliament:Pododeljak">
                            <h5> <xsl:value-of select="//parliament:NazivPododeljka"/></h5>
                            <h6> Pododeljak <xsl:value-of select="//@parliament:RedniBroj"/> </h6>
                            <xsl:for-each select="//parliament:Clan">
                              <h6> <xsl:value-of select="//parliament:NazivClana"/> </h6>
                              <h6> Član <xsl:value-of select="//@parliament:RedniBroj"/> </h6>

                              <xsl:choose>
                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                  <p>  <xsl:value-of select="current()"/> </p>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:for-each select="//parliament:Stav">
                                    <xsl:choose>
                                      <xsl:when test="//parliament:TekstualniSadrzaj">
                                        <p>  <xsl:value-of select="current()"/> </p>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        <xsl:for-each select="//parliament:Tacka">
                                          <xsl:choose>
                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                              <p>  <xsl:value-of select="current()"/> </p>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:for-each select="//parliament:Podtacka">
                                                <xsl:choose>
                                                  <xsl:when test="//parliament:TekstualniSadrzaj">
                                                    <p>  <xsl:value-of select="current()"/> </p>
                                                  </xsl:when>
                                                  <xsl:otherwise>
                                                    <xsl:for-each select="//parliament:Alineja">
                                                      <p> Sadržaj alineje: <xsl:value-of select="current()"/> </p>
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
                              <h6> <xsl:value-of select="//parliament:NazivClana"/> </h6>
                              <h6> Član <xsl:value-of select="//@parliament:RedniBroj"/> </h6>

                              <xsl:choose>
                                <xsl:when test="//parliament:TekstualniSadrzaj">
                                  <p>  <xsl:value-of select="current()"/> </p>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:for-each select="//parliament:Stav">
                                    <xsl:choose>
                                      <xsl:when test="//parliament:TekstualniSadrzaj">
                                        <p>  <xsl:value-of select="current()"/> </p>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        <xsl:for-each select="//parliament:Tacka">
                                          <xsl:choose>
                                            <xsl:when test="//parliament:TekstualniSadrzaj">
                                              <p>  <xsl:value-of select="current()"/> </p>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:for-each select="//parliament:Podtacka">
                                                <xsl:choose>
                                                  <xsl:when test="//parliament:TekstualniSadrzaj">
                                                    <p>  <xsl:value-of select="current()"/> </p>
                                                  </xsl:when>
                                                  <xsl:otherwise>
                                                    <xsl:for-each select="//parliament:Alineja">
                                                      <p> Sadržaj alineje: <xsl:value-of select="current()"/> </p>
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
                      <h6> <xsl:value-of select="//parliament:NazivClana"/> </h6>
                      <h6> Član <xsl:value-of select="//@parliament:RedniBroj"/> </h6>

                      <xsl:choose>
                        <xsl:when test="//parliament:TekstualniSadrzaj">
                          <p>  <xsl:value-of select="current()"/> </p>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:for-each select="//parliament:Stav">
                            <xsl:choose>
                              <xsl:when test="//parliament:TekstualniSadrzaj">
                                <p>  <xsl:value-of select="current()"/> </p>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:for-each select="//parliament:Tacka">
                                  <xsl:choose>
                                    <xsl:when test="//parliament:TekstualniSadrzaj">
                                      <p>  <xsl:value-of select="current()"/> </p>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:for-each select="//parliament:Podtacka">
                                        <xsl:choose>
                                          <xsl:when test="//parliament:TekstualniSadrzaj">
                                            <p>  <xsl:value-of select="current()"/> </p>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:for-each select="//parliament:Alineja">
                                              <p> Sadržaj alineje: <xsl:value-of select="current()"/> </p>
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
                    <h3> Naziv Priloga: <xsl:value-of select="current()"/>
                    </h3>
                </xsl:for-each>
            </div>
        </body>
      </html>
    </xsl:template>
</xsl:stylesheet>