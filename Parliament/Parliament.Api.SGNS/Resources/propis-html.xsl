<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:parliament="http://www.parliament.rs/schema"
    exclude-result-prefixes="xs"
    version="2.0">
    <xsl:template match="/">
        <body>
            <div class="container">
                <h1><xsl:value-of select="/*/NazivPropisa"/></h1>
            </div>
        </body>
    </xsl:template>
</xsl:stylesheet>