xquery version "1.0-ml";

import schema "http://www.parliament.rs/schema" at "Amandman.xsd";
declare namespace parliament = "http://www.parliament.rs/schema";

declare variable $amandment_string as xs:string external;
declare variable $id as xs:string external;
declare variable $amandment := xdmp:unquote($amandment_string);
declare variable $errors := xdmp:validate($amandment, "strict");
declare variable $validation_error := if (exists($errors//error:error)) then "Validation Error" else "Success";
declare variable $document_uri := "http://www.parliament.rs/documents/amandments/" || $id || ".xml";

declare variable $result := if ($validation_error eq 'Success') then
  try {
    if (empty(xdmp:document-insert($document_uri, $amandment, xdmp:default-permissions()))) then $document_uri else 'Document Loading Error'
  }
  catch($exception){
    'Document Loading Error'
  }
else $errors || "Error";

declare variable $props := xdmp:document-add-properties($document_uri,(
  <IdPropisa>{data($amandment/parliament:Amandman/@PredmetAmandmana)}</IdPropisa>,
  <DatumVremePredlaganja>{data($amandment/parliament:Amandman/@parliament:DatumVremePredlaganja)}</DatumVremePredlaganja>,
  <DatumVremeUsvajanja>{data($amandment/parliament:Amandman/@parliament:DatumVremeUsvajanja)}</DatumVremeUsvajanja>,
  <ImeNadleznogOrgana>{data($amandment/parliament:Amandman/parliament:NadlezniOrgan/parliament:Ime)}</ImeNadleznogOrgana>,
  <PrezimeNadleznogOrgana>{data($amandment/parliament:Amandman/parliament:NadlezniOrgan/parliament:Prezime)}</PrezimeNadleznogOrgana>,
  <EmailNadleznogOrgana>{data($amandment/parliament:Amandman/parliament:NadlezniOrgan/parliament:Email)}</EmailNadleznogOrgana>,
  <SerijskiBroj>{data($amandment/parliament:Amandman/@SerijskiBroj)}</SerijskiBroj>
));

if (empty($props)) then $result else "Property Error"