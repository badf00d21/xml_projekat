xquery version "1.0-ml";

import schema "http://www.parliament.rs/schema" at "Propis.xsd";
declare namespace parliament = "http://www.parliament.rs/schema";

declare variable $act_string as xs:string external;
declare variable $act := xdmp:unquote($act_string);
declare variable $errors := xdmp:validate($act, "strict");
declare variable $validation_error := if (exists($errors//error:error)) then "Validation Error" else "Success";
declare variable $document_uri := "http://www.parliament.rs/documents/acts/" || sem:uuid-string() || ".xml";

declare variable $result := if ($validation_error eq 'Success') then
  try {
    if (empty(xdmp:document-insert($document_uri, $act, xdmp:default-permissions()))) then $document_uri else 'Document Loading Error'
  }
  catch($exception){
    'Document Loading Error'
  }
else $errors || "Error";

declare variable $props := xdmp:document-add-properties($document_uri,(
  <NazivPropisa>{data($act/parliament:Propis/parliament:NazivPropisa)}</NazivPropisa>,
  <DatumVremePredlaganja>{data($act/parliament:Propis/@parliament:DatumVremePredlaganja)}</DatumVremePredlaganja>,
  <DatumVremeUsvajanja>{data($act/parliament:Propis/@parliament:DatumVremeUsvajanja)}</DatumVremeUsvajanja>,
  <ImeNadleznogOrgana>{data($act/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan/parliament:Ime)}</ImeNadleznogOrgana>,
  <PrezimeNadleznogOrgana>{data($act/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan/parliament:Prezime)}</PrezimeNadleznogOrgana>,
  <EmailNadleznogOrgana>{data($act/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan/parliament:Email)}</EmailNadleznogOrgana>,
  <SerijskiBroj>{data($act/parliament:Propis/@SerijskiBroj)}</SerijskiBroj>,
  <Status>{'Predlozen'}</Status>
));

if (empty($props)) then $result else "Property Error"