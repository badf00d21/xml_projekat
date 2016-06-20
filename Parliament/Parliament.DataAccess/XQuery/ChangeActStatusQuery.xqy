xquery version "1.0-ml";

declare variable $document as xs:string external;
declare variable $status as xs:string external;

xdmp:document-set-property($document, <Status>{$status}</Status>);