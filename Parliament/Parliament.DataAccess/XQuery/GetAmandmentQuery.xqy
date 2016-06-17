declare variable $text as xs:string external;
declare variable $id_propisa as xs:string external;
declare variable $datum_vreme_predlaganja as xs:string external;
declare variable $datum_vreme_usvajanja as xs:string external;
declare variable $serijski_broj as xs:string external;
declare variable $ime_nadleznog_organa as xs:string external;
declare variable $prezime_nadleznog_organa as xs:string external;
declare variable $email_nadleznog_organa as xs:string external;

declare variable $q1 := cts:directory-query("http://www.parliament.rs/documents/amandments/", "infinity");

declare variable $q2 := if ($text eq "") then $q1 else
    cts:and-query(($q1,
    cts:word-query(
            tokenize($text, '\s'),
            "case-insensitive")
    ));


declare variable $q4 := if ($datum_vreme_predlaganja eq "") then $q2 else
    cts:and-query(($q2,
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

declare variable $q7 := if ($id_propisa eq "") then $q6 else
    cts:and-query(($q6,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'IdPropisa'),
                    tokenize($id_propisa, '\s')
            )
    )));

declare variable $q8 := if ($ime_nadleznog_organa eq "") then $q7 else
    cts:and-query(($q7,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'ImeNadleznogOrgana'),
                    tokenize($ime_nadleznog_organa, '\s')
            )
    )));

declare variable $q9 := if ($prezime_nadleznog_organa eq "") then $q8 else
    cts:and-query(($q8,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'PrezimeNadleznogOrgana'),
                    tokenize($prezime_nadleznog_organa, '\s')
            )
    )));

declare variable $q10 := if ($email_nadleznog_organa eq "") then $q9 else
    cts:and-query(($q9,
    cts:properties-query(
            cts:element-word-query(
                    QName('', 'EmailNadleznogOrgana'),
                    tokenize($email_nadleznog_organa, '\s')
            )
    )));



<Amandmani>
    {
        for $x in cts:uris((), (), $q10)
        return
            <Amandman>
                <Id>{(tokenize(((tokenize($x, "[/]"))[last()]), "[.]"))[1]}</Id>
                <IdPropisa>{data(xdmp:document-get-properties($x, QName('', 'IdPropisa')))}</IdPropisa>
				<ImeNadleznogOrgana>{data(xdmp:document-get-properties($x, QName('', 'ImeNadleznogOrgana')))}</ImeNadleznogOrgana>
				<PrezimeNadleznogOrgana>{data(xdmp:document-get-properties($x, QName('', 'PrezimeNadleznogOrgana')))}</PrezimeNadleznogOrgana>
				<EmailNadleznogOrgana>{data(xdmp:document-get-properties($x, QName('', 'EmailNadleznogOrgana')))}</EmailNadleznogOrgana>
            </Amandman>
    }
</Amandmani>