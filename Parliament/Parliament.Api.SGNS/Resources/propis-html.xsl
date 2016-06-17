<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:parliament="http://www.parliament.rs/schema" exclude-result-prefixes="xs" version="2.0">
    <xsl:template match="/"> <!-- Ovo Matchuje dokument i postavlja ga za trenutni kontekst, svaka sledeca putanja do taka je relativna u odnosu na ovo -->
        <body>
            <div class="container">
                <h1><xsl:value-of select="parliament:Propis/parliament:NazivPropisa"/></h1> <!-- Za naslov dokumenta ide h1 i izvlaci se vrednost naslova iz xml-a, obratite paznju na putanju (prefiks:ImeTaga/prefiks:PodTag) -->
                <xsl:for-each select="//parliament:Preambula"> <!-- Putanja do preambule ili ovako jer je trenutni kontekst Propis ili moze full putanja parliament:Propis/parliament:Preambula -->
                    <xsl:for-each select="//parliament:PravniOsnov">
                        <p> Pravni osnov: <xsl:value-of select="current()"/> </p> <!-- Ovde je u for-each selektovan PravniPropis tag pa njegova vrednost se dobija sa current(), jer nema podtagove -->
                    </xsl:for-each>
                    <xsl:for-each select="//parliament:Preambula/parliament:NadlezniOrgan"> <!-- Ovde je bitno staviti u putanju i preambulu jer nadlezni organ se nalazi i u tagu Saglasnost pa da ne bi bilo duplo -->
                        <p> Ime nadleznog organa: <xsl:value-of select="//parliament:Ime"/> </p>
                        <p> Prezime nadleznog organa: <xsl:value-of select="//parliament:Prezime"/> </p>
                        <p> Email nadleznog organa: <xsl:value-of select="//parliament:Email"/> </p>
                    </xsl:for-each>
                    <xsl:for-each select="//parliament:Saglasnost">
                        <br/>
                        <xsl:for-each select="//parliament:Naznaka">
                            <p> Naznaka saglasnosti: <xsl:value-of select="current()"/> </p> 
                        </xsl:for-each>
                        <xsl:for-each select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan"> <!-- Ovde sad kazemo da hocemo NadlezniOrgan iz taga Saglasnost -->
                            <p> Ime nadleznog organa: <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Ime"/> </p>
                            <p> Prezime nadleznog organa: <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Prezime"/> </p>
                            <p> Email nadleznog organa: <xsl:value-of select="//parliament:Preambula/parliament:Saglasnost/parliament:NadlezniOrgan/parliament:Email"/> </p>
                        </xsl:for-each>
                    </xsl:for-each>
                 </xsl:for-each>   
            </div>
        </body>
    </xsl:template>
</xsl:stylesheet>