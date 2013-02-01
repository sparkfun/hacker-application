// ** I18N

// Calendar FI language
// Author: Antti Tuppurainen, <antti.tuppurainen@systemprofes.fi>
// Encoding: UTF-8
// Distributed under the same terms as the calendar itself.

// For translators: please use UTF-8 if possible.  We strongly believe that
// Unicode is the answer to a real internationalized world.  Also please
// include your contact information in the header, as can be seen above.

// full day names
Calendar._DN = new Array
("Sunnuntai",
 "Maanantai",
 "Tiistai",
 "Keskiviikko",
 "Torstai",
 "Perjantai",
 "Lauantai",
 "Sunnuntai");

// Please note that the following array of short day names (and the same goes
// for short month names, _SMN) isn't absolutely necessary.  We give it here
// for exemplification on how one can customize the short day names, but if
// they are simply the first N letters of the full name you can simply say:
//
//   Calendar._SDN_len = N; // short day name length
//   Calendar._SMN_len = N; // short month name length
//
// If N = 3 then this is not needed either since we assume a value of 3 if not
// present, to be compatible with translation files that were written before
// this feature.

// short day names
Calendar._SDN = new Array
("Su",
 "Ma",
 "Ti",
 "Ke",
 "To",
 "Pe",
 "La",
 "Su");

// full month names
Calendar._MN = new Array
("Tammikuu",
 "Helmikuu",
 "Maaliskuu",
 "Huhtikuu",
 "Toukokuu",
 "Keso?=uu",
 "HeinC$uu",
 "Elokuu",
 "Syyskuu",
 "Lokakuu",
 "Marraskuu",
 "Joulukuu");

// short month names
Calendar._SMN = new Array
("Tammi",
 "Helmi",
 "Maalis",
 "Huhti",
 "Touko",
 "KesC$",
 "HeinC$",
 "Elo",
 "Syys",
 "Loka",
 "Marras",
 "Joulu");

// tooltips
Calendar._TT = {};
Calendar._TT["INFO"] = "Tietoa kalenterista";

Calendar._TT["ABOUT"] =
"DHTML Date/Time Selector\n" +
"(c) dynarch.com 2002-2003\n" + // don't translate this this ;-)
"Viimeisin versio kalenterista: http://dynarch.com/mishoo/calendar.epl\n" +
"Julkaistu GNU LGPL -lisenssillC$ Ehdot: http://gnu.org/licenses/lgpl.html." +
"\n\n" +
"PC$ivC$mC$C$rC$n valinta:\n" +
"- KC$ytC$ \xab, \xbb -linkkejC$ vuoden valinnassa\n" +
"- KC$ytC$ " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) + " -linkkejC$ kuukauden valinnassa\n" +
"- PidC$ hiiren nappi alhaalla nopeuttaaksesi valintaa.";
Calendar._TT["ABOUT_TIME"] = "\n\n" +
"Kellonajan valinta:\n" +
"- Klikkaa mitC$ tahansa osaa kellosta suurentaaksesi lukua\n" +
"- tai Shifti+klikkaus vC$hentC$C$ksesi sitC$" +
"- tai klikkaa ja vedC$ nopeuttaksesi valintaa.";

Calendar._TT["PREV_YEAR"] = "Edellinen vuosi (valikko)";
Calendar._TT["PREV_MONTH"] = "Edellinen kuukausi (valikko)";
Calendar._TT["GO_TODAY"] = "Siirry pC$ivC$C$n";
Calendar._TT["NEXT_MONTH"] = "Seuraava kuukausi (valikko)";
Calendar._TT["NEXT_YEAR"] = "Seuraava vuosi (valikko)";
Calendar._TT["SEL_DATE"] = "Valitse pC$ivC$mC$C$rC$";
Calendar._TT["DRAG_TO_MOVE"] = "VedC$ siirtC$C$ksesi";
Calendar._TT["PART_TODAY"] = " (TC$mC$ pC$ivC$)";
Calendar._TT["MON_FIRST"] = "NC$ytC$ maanantai ensimmC$isenC$";
Calendar._TT["SUN_FIRST"] = "NC$ytC$ sunnuntai ensimmC$isenC$";
Calendar._TT["CLOSE"] = "Sulje";
Calendar._TT["TODAY"] = "TC$nC$C$n";
Calendar._TT["TIME_PART"] = "(Shifti-)Klikkaus tai hiiren vetC$minen vaihtaa arvoa";

// date formats
Calendar._TT["DEF_DATE_FORMAT"] = "%d.%m.%Y";
Calendar._TT["TT_DATE_FORMAT"] = "%a, %b %e";

Calendar._TT["WK"] = "vko";
