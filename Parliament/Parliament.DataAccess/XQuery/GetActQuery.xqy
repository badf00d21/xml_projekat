declare variable $text as xs:string external;
declare variable $naziv_propisa as xs:string external;
declare variable $datum_vreme_predlaganja as xs:string external;
declare variable $datum_vreme_usvajanja as xs:string external;
declare variable $serijski_broj as xs:string external;
declare variable $status as xs:string external;

declare variable $q1 := cts:directory-query("http://www.parliament.rs/documents/acts/", "infinity");

declare variable $q2 := if ($text eq "") then $q1 else
    cts:and-query(($q1,
    cts:word-query(
            tokenize($text, '\s'),
            "case-insensitive")
    ));

declare variable $q3 := if ($naziv_propisa eq "") then $q2 else
    cts:and-query(($q2,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'NazivPropisa'),
                    tokenize($naziv_propisa, '\s')
            )
    )));

declare variable $q4 := if ($datum_vreme_predlaganja eq "") then $q3 else
    cts:and-query(($q3,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'DatumVremePredlaganja'),
                    tokenize($datum_vreme_predlaganja, '\s')
            )
    )));

declare variable $q5 := if ($datum_vreme_usvajanja eq "") then $q4 else
    cts:and-query(($q4, cts:properties-query(
            cts:element-word-query(
                    QName('', 'DatumVremeUsvajanja'),
                    tokenize($datum_vreme_usvajanja, '\s')
            )
    )));

declare variable $q6 := if ($serijski_broj eq "") then $q5 else
    cts:and-query(($q5,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'SerijskiBroj'),
                    tokenize($serijski_broj, '\s')
            )
    )));

declare variable $q7 := if ($status eq "") then $q6 else
    cts:and-query(($q6,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'Status'),
                    tokenize($status, '\s')
            )
    )));

<Propisi>
    {
        for $x in cts:uris((), (), $q7)
        return
            <Propis>
                <uri>{$x}</uri>
                <NazivPropisa>{data(xdmp:document-get-properties($x, QName('', 'NazivPropisa')))}</NazivPropisa>
                <Status>{data(xdmp:document-get-properties($x, QName('', 'Status')))}</Status>
            </Propis>
    }
</Propisi>