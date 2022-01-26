!function(e){"use strict";"function"==typeof define&&define.amd?define(["jquery","./grid.utils","./grid.base"],e):e(jQuery)}(function(G){"use strict";G.jgrid=G.jgrid||{},G.extend(G.jgrid,{saveState:function(e,t){if(t=G.extend({useStorage:!0,storageType:"localStorage",beforeSetItem:null,compression:!1,compressionModule:"LZString",compressionMethod:"compressToUTF16",debug:!1,saveData:!0},t||{}),e){var r,i="",o="",a=G("#"+e)[0];if(a.grid){if((e=G(a).data("inlineNav"))&&a.p.inlineNav&&G(a).jqGrid("setGridParam",{_iN:e}),(e=G(a).data("filterToolbar"))&&a.p.filterToolbar&&G(a).jqGrid("setGridParam",{_fT:e}),i=G(a).jqGrid("jqGridExport",{exptype:"jsonstring",ident:"",root:"",data:t.saveData}),o="",t.saveData&&(d=(o=G(a.grid.bDiv).find(".ui-jqgrid-btable tbody").first().html()).indexOf("</tr>"),o=o.slice(d+5)),G.jgrid.isFunction(t.beforeSetItem)&&null!=(r=t.beforeSetItem.call(a,i))&&(i=r),t.debug){G("#gbox_tree").prepend('<a id="link_save" target="_blank" download="jqGrid_dump.txt">Click to save Dump Data</a>');var n,d,s=[],l={};s.push("Grid Options\n"),s.push(i),s.push("\n"),s.push("GridData\n"),s.push(o),l.type="plain/text;charset=utf-8";try{n=new File(s,"jqGrid_dump.txt",l)}catch(e){n=new Blob(s,l)}d=URL.createObjectURL(n),G("#link_save").attr("href",d).on("click",function(){G(this).remove()})}if(t.compression&&t.compressionModule)try{null!=(r=window[t.compressionModule][t.compressionMethod](i))&&(i=r,o=window[t.compressionModule][t.compressionMethod](o))}catch(e){}if(t.useStorage&&G.jgrid.isLocalStorage())try{window[t.storageType].setItem("jqGrid"+a.p.id,i),window[t.storageType].setItem("jqGrid"+a.p.id+"_data",o)}catch(e){22===e.code&&alert("Local storage limit is over!")}return i}}},loadState:function(e,t,r){if(r=G.extend({useStorage:!0,storageType:"localStorage",clearAfterLoad:!1,beforeSetGrid:null,afterSetGrid:null,decompression:!1,decompressionModule:"LZString",decompressionMethod:"decompressFromUTF16",restoreData:!0},r||{}),e){var i,o,a,n,d=G("#"+e)[0];if(r.useStorage)try{t=window[r.storageType].getItem("jqGrid"+d.id),o=window[r.storageType].getItem("jqGrid"+d.id+"_data")}catch(e){}if(t){if(r.decompression&&r.decompressionModule)try{null!=(i=window[r.decompressionModule][r.decompressionMethod](t))&&(t=i,o=window[r.decompressionModule][r.decompressionMethod](o))}catch(e){}if((i=G.jgrid.parseFunc(t))&&"object"===G.jgrid.type(i)){d.grid&&G.jgrid.gridUnload(e),G.jgrid.isFunction(r.beforeSetGrid)&&(l=r.beforeSetGrid(i))&&"object"===G.jgrid.type(l)&&(i=l);var s=function(e){return e},l={reccount:i.reccount,records:i.records,lastpage:i.lastpage,shrinkToFit:s(i.shrinkToFit),data:s(i.data),datatype:s(i.datatype),grouping:s(i.grouping)};i.shrinkToFit=!1,i.data=[],i.datatype="local",i.grouping=!1,i.inlineNav&&(a=s(i._iN),i._iN=null,delete i._iN),i.filterToolbar&&(n=s(i._fT),i._fT=null,delete i._fT);var p=G("#"+e).jqGrid(i);if(p.jqGrid("delRowData","norecs"),r.restoreData&&""!==G.jgrid.trim(o)&&p.append(o),p.jqGrid("setGridParam",l),i.storeNavOptions&&i.navGrid&&(p[0].p.navGrid=!1,p.jqGrid("navGrid",i.pager,i.navOptions,i.editOptions,i.addOptions,i.delOptions,i.searchOptions,i.viewOptions),i.navButtons&&i.navButtons.length))for(var c=0;c<i.navButtons.length;c++)"sepclass"in i.navButtons[c][1]?p.jqGrid("navSeparatorAdd",i.navButtons[c][0],i.navButtons[c][1]):p.jqGrid("navButtonAdd",i.navButtons[c][0],i.navButtons[c][1]);if(p[0].refreshIndex(),i.subGrid&&(s=1===i.multiselect?1:0,l=!0===i.rownumbers?1:0,p.jqGrid("addSubGrid",s+l),G.each(p[0].rows,function(e,t){G(t).hasClass("ui-sg-expanded")&&G(p[0].rows[e-1]).find("td.sgexpanded").click().click()})),i.treeGrid)for(var g=1,u=p[0].rows.length,m=i.expColInd,f=i.treeReader.leaf_field,j=i.treeReader.expanded_field;g<u;)G(p[0].rows[g].cells[m]).find("div.treeclick").on("click",function(e){e=e.target||e.srcElement,e=G.jgrid.stripPref(i.idPrefix,G(e,p[0].rows).closest("tr.jqgrow")[0].id),e=p[0].p._index[e];return p[0].p.data[e][f]||(p[0].p.data[e][j]?(p.jqGrid("collapseRow",p[0].p.data[e]),p.jqGrid("collapseNode",p[0].p.data[e])):(p.jqGrid("expandRow",p[0].p.data[e]),p.jqGrid("expandNode",p[0].p.data[e]))),!1}),!0===i.ExpandColClick&&G(p[0].rows[g].cells[m]).find("span.cell-wrapper").css("cursor","pointer").on("click",function(e){var t=e.target||e.srcElement,e=G.jgrid.stripPref(i.idPrefix,G(t,p[0].rows).closest("tr.jqgrow")[0].id),t=p[0].p._index[e];return p[0].p.data[t][f]||(p[0].p.data[t][j]?(p.jqGrid("collapseRow",p[0].p.data[t]),p.jqGrid("collapseNode",p[0].p.data[t])):(p.jqGrid("expandRow",p[0].p.data[t]),p.jqGrid("expandNode",p[0].p.data[t]))),p.jqGrid("setSelection",e),!1}),g++;i.multiselect&&G.each(i.selarrrow,function(){G("#jqg_"+e+"_"+this)[i.useProp?"prop":"attr"]("checked","checked")}),i.inlineNav&&a&&(p.jqGrid("setGridParam",{inlineNav:!1}),p.jqGrid("inlineNav",i.pager,a)),i.filterToolbar&&n&&(p.jqGrid("setGridParam",{filterToolbar:!1}),n.restoreFromFilters=!0,p.jqGrid("filterToolbar",n)),i.frozenColumns&&p.jqGrid("setFrozenColumns"),p[0].updatepager(!0,!0),G.jgrid.isFunction(r.afterSetGrid)&&r.afterSetGrid(p),r.clearAfterLoad&&(window[r.storageType].removeItem("jqGrid"+d.id),window[r.storageType].removeItem("jqGrid"+d.id+"_data"))}else alert("can not convert to object")}}},isGridInStorage:function(e,t){var r,i,o,a={storageType:"localStorage"},a=G.extend(a,t||{});try{i=window[a.storageType].getItem("jqGrid"+e),o=window[a.storageType].getItem("jqGrid"+e+"_data"),r=null!=i&&null!=o&&"string"==typeof i&&"string"==typeof o}catch(e){r=!1}return r},setRegional:function(e,t){var r={storageType:"sessionStorage"};if((r=G.extend(r,t||{})).regional){G.jgrid.saveState(e,r),r.beforeSetGrid=function(e){return e.regional=r.regional,e.force_regional=!0,e},G.jgrid.loadState(e,null,r);var i=G("#"+e)[0],t=G(i).jqGrid("getGridParam","colModel"),o=-1,a=G.jgrid.getRegional(i,"nav");G.each(t,function(e){if(this.formatter&&"actions"===this.formatter)return o=e,!1}),-1!==o&&a&&G("#"+e+" tbody tr").each(function(){var e=this.cells[o];G(e).find(".ui-inline-edit").attr("title",a.edittitle),G(e).find(".ui-inline-del").attr("title",a.deltitle),G(e).find(".ui-inline-save").attr("title",a.savetitle),G(e).find(".ui-inline-cancel").attr("title",a.canceltitle)});try{window[r.storageType].removeItem("jqGrid"+i.id),window[r.storageType].removeItem("jqGrid"+i.id+"_data")}catch(e){}}},jqGridImport:function(e,r){r=G.extend({imptype:"xml",impstring:"",impurl:"",mtype:"GET",impData:{},xmlGrid:{config:"root>grid",data:"root>rows"},jsonGrid:{config:"grid",data:"data"},ajaxOptions:{}},r||{});function i(e,t){var r,i,o,a=G(t.xmlGrid.config,e)[0],t=G(t.xmlGrid.data,e)[0];if(G.grid.xmlToJSON){for(o in r=G.jgrid.xmlToJSON(a))r.hasOwnProperty(o)&&(i=r[o]);t?(t=r.grid.datatype,r.grid.datatype="xmlstring",r.grid.datastr=e,G(n).jqGrid(i).jqGrid("setGridParam",{datatype:t})):setTimeout(function(){G(n).jqGrid(i)},0)}else alert("xml2json or parse are not present")}function t(e,t){var r;e&&"string"==typeof e&&(e=(r=G.jgrid.parseFunc(e))[t.jsonGrid.config],(r=r[t.jsonGrid.data])?(t=e.datatype,e.datatype="jsonstring",e.datastr=r,G(n).jqGrid(e).jqGrid("setGridParam",{datatype:t})):G(n).jqGrid(e))}var o,n=(0===e.indexOf("#")?"":"#")+G.jgrid.jqID(e);switch(r.imptype){case"xml":G.ajax(G.extend({url:r.impurl,type:r.mtype,data:r.impData,dataType:"xml",complete:function(e,t){"success"===t&&(i(e.responseXML,r),G(n).triggerHandler("jqGridImportComplete",[e,r]),G.jgrid.isFunction(r.importComplete)&&r.importComplete(e)),e=null}},r.ajaxOptions));break;case"xmlstring":!r.impstring||"string"!=typeof r.impstring||(o=G.parseXML(r.impstring))&&(i(o,r),G(n).triggerHandler("jqGridImportComplete",[o,r]),G.jgrid.isFunction(r.importComplete)&&r.importComplete(o));break;case"json":G.ajax(G.extend({url:r.impurl,type:r.mtype,data:r.impData,dataType:"json",complete:function(e){try{t(e.responseText,r),G(n).triggerHandler("jqGridImportComplete",[e,r]),G.jgrid.isFunction(r.importComplete)&&r.importComplete(e)}catch(e){}e=null}},r.ajaxOptions));break;case"jsonstring":r.impstring&&"string"==typeof r.impstring&&(t(r.impstring,r),G(n).triggerHandler("jqGridImportComplete",[r.impstring,r]),G.jgrid.isFunction(r.importComplete)&&r.importComplete(r.impstring))}}}),G.jgrid.extend({jqGridExport:function(t){t=G.extend({exptype:"xmlstring",root:"grid",ident:"\t",addOptions:{},data:!0},t||{});var r=null;return this.each(function(){if(this.grid){var e=G.extend(!0,{},G(this).jqGrid("getGridParam"),t.addOptions);switch(e.rownumbers&&(e.colNames.splice(0,1),e.colModel.splice(0,1)),e.multiselect&&(e.colNames.splice(0,1),e.colModel.splice(0,1)),e.subGrid&&(e.colNames.splice(0,1),e.colModel.splice(0,1)),e.knv=null,t.data||(e.data=[],e._index={}),t.exptype){case"xmlstring":r="<"+t.root+">"+G.jgrid.jsonToXML(e,{xmlDecl:""})+"</"+t.root+">";break;case"jsonstring":r=G.jgrid.stringify(e),t.root&&(r="{"+t.root+":"+r+"}")}}}),r},excelExport:function(d){return d=G.extend({exptype:"remote",url:null,oper:"oper",tag:"excel",beforeExport:null,exporthidden:!1,exportgrouping:!1,exportOptions:{}},d||{}),this.each(function(){if(this.grid&&"remote"===d.exptype){var e,t=G.extend({},this.p.postData);if(t[d.oper]=d.tag,G.jgrid.isFunction(d.beforeExport)&&(n=d.beforeExport.call(this,t),G.isPlainObject(n)&&(t=n)),d.exporthidden){for(var r=this.p.colModel,i=r.length,o=[],a=0;a<i;a++)void 0===r[a].hidden&&(r[a].hidden=!1),o.push({name:r[a].name,hidden:r[a].hidden});var n=JSON.stringify(o);"string"==typeof n&&(t.colModel=n)}d.exportgrouping&&"string"==typeof(e=JSON.stringify(this.p.groupingView))&&(t.groupingView=e);var t=jQuery.param(t),t=-1!==d.url.indexOf("?")?d.url+"&"+t:d.url+"?"+t;window.location=t}})}})});