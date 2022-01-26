!function(i){"use strict";"function"==typeof define&&define.amd?define(["jquery","./grid.base"],i):i(jQuery)}(function(j){"use strict";j.jgrid.extend({setSubGrid:function(){return this.each(function(){var i,e,s=this,d=j.jgrid.styleUI[s.p.styleUI||"jQueryUI"].subgrid,d={plusicon:d.icon_plus,minusicon:d.icon_minus,openicon:d.icon_open,expandOnLoad:!1,selectOnExpand:!1,selectOnCollapse:!1,reloadOnExpand:!0};if(s.p.subGridOptions=j.extend(d,s.p.subGridOptions||{}),s.p.colNames.unshift(""),s.p.colModel.unshift({name:"subgrid",width:j.jgrid.cell_width?s.p.subGridWidth+s.p.cellLayout:s.p.subGridWidth,sortable:!1,resizable:!1,hidedlg:!0,search:!1,fixed:!0}),(i=s.p.subGridModel)[0])for(i[0].align=j.extend([],i[0].align||[]),e=0;e<i[0].name.length;e++)i[0].align[e]=i[0].align[e]||"left"})},addSubGridCell:function(i,e){var s,d,r,t="";return this.each(function(){t=this.formatCol(i,e),d=this.p.id,s=this.p.subGridOptions.plusicon,r=j.jgrid.styleUI[this.p.styleUI||"jQueryUI"].common}),'<td role="gridcell" aria-describedby="'+d+'_subgrid" class="ui-sgcollapsed sgcollapsed" '+t+"><a style='cursor:pointer;' class='ui-sghref'><span class='"+r.icon_base+" "+s+"'></span></a></td>"},addSubGrid:function(G,m){return this.each(function(){var u=this;if(u.grid){var e,s,d,r,t,c=j.jgrid.styleUI[u.p.styleUI||"jQueryUI"].base,g=j.jgrid.styleUI[u.p.styleUI||"jQueryUI"].common,b=function(i,e,s){e=j("<td align='"+u.p.subGridModel[0].align[s]+"'></td>").html(e);j(i).append(e)},n=function(i,e){for(var s,d,r=j("<table class='"+c.rowTable+" ui-common-table'><tbody></tbody></table>"),t=j("<tr></tr>"),n=0;n<u.p.subGridModel[0].name.length;n++)s=j("<th class='"+c.headerBox+" ui-th-subgrid ui-th-column ui-th-"+u.p.direction+"'></th>"),j(s).html(u.p.subGridModel[0].name[n]),j(s).width(u.p.subGridModel[0].width[n]),j(t).append(s);j(r).append(t),i&&(d=u.p.xmlReader.subgrid,j(d.root+" "+d.row,i).each(function(){if(t=j("<tr class='"+g.content+" ui-subtblcell'></tr>"),!0===d.repeatitems)j(d.cell,this).each(function(i){b(t,j(this).text()||"&#160;",i)});else{var i=u.p.subGridModel[0].mapping||u.p.subGridModel[0].name;if(i)for(n=0;n<i.length;n++)b(t,j.jgrid.getXmlData(this,i[n])||"&#160;",n)}j(r).append(t)}));i=j(u.grid.bDiv).find("table").first().attr("id")+"_";return j("#"+j.jgrid.jqID(i+e)).append(r),u.grid.hDiv.loading=!1,j("#load_"+j.jgrid.jqID(u.p.id)).hide(),!1},a=function(i,e){for(var s,d,r,t,n,a=j("<table class='"+c.rowTable+" ui-common-table'><tbody></tbody></table>"),l=j("<tr></tr>"),o=0;o<u.p.subGridModel[0].name.length;o++)s=j("<th class='"+c.headerBox+" ui-th-subgrid ui-th-column ui-th-"+u.p.direction+"'></th>"),j(s).html(u.p.subGridModel[0].name[o]),j(s).width(u.p.subGridModel[0].width[o]),j(l).append(s);if(j(a).append(l),i&&(t=u.p.jsonReader.subgrid,void 0!==(d=j.jgrid.getAccessor(i,t.root))))for(o=0;o<d.length;o++){if(r=d[o],l=j("<tr class='"+g.content+" ui-subtblcell'></tr>"),!0===t.repeatitems)for(t.cell&&(r=r[t.cell]),n=0;n<r.length;n++)b(l,r[n]||"&#160;",n);else{var p=u.p.subGridModel[0].mapping||u.p.subGridModel[0].name;if(p.length)for(n=0;n<p.length;n++)b(l,j.jgrid.getAccessor(r,p[n])||"&#160;",n)}j(a).append(l)}i=j(u.grid.bDiv).find("table").first().attr("id")+"_";return j("#"+j.jgrid.jqID(i+e)).append(a),u.grid.hDiv.loading=!1,j("#load_"+j.jgrid.jqID(u.p.id)).hide(),!1},l=0;j.each(u.p.colModel,function(){!0!==this.hidden&&"rn"!==this.name&&"cb"!==this.name||l++});var i,o,p=u.rows.length,h=1,f=j.jgrid.isFunction(u.p.isHasSubGrid);for(void 0!==m&&0<m&&(p=(h=m)+1);h<p;)j(u.rows[h]).hasClass("jqgrow")&&(u.p.scroll&&j(u.rows[h].cells[G]).off("click"),i=null,f&&(i=u.p.isHasSubGrid.call(u,u.rows[h].id)),!1===i?u.rows[h].cells[G].innerHTML="":j(u.rows[h].cells[G]).on("click",function(){var i=j(this).parent("tr")[0];if(s=u.p.id,e=i.id,t=j("#"+s+"_"+e+"_expandedContent"),j(this).hasClass("sgcollapsed")){if((r=!1!==(r=j(u).triggerHandler("jqGridSubGridBeforeExpand",[s+"_"+e,e]))&&"stop"!==r)&&j.jgrid.isFunction(u.p.subGridBeforeExpand)&&(r=u.p.subGridBeforeExpand.call(u,s+"_"+e,e)),!1===r)return!1;!0===u.p.subGridOptions.reloadOnExpand||!1===u.p.subGridOptions.reloadOnExpand&&!t.hasClass("ui-subgrid")?(d=1<=G?"<td colspan='"+G+"'>&#160;</td>":"",j(i).after("<tr role='row' id='"+s+"_"+e+"_expandedContent' class='ui-subgrid ui-sg-expanded'>"+d+"<td class='"+g.content+" subgrid-cell'><span class='"+g.icon_base+" "+u.p.subGridOptions.openicon+"'></span></td><td colspan='"+parseInt(u.p.colNames.length-1-l,10)+"' class='"+g.content+" subgrid-data'><div id="+s+"_"+e+" class='tablediv'></div></td></tr>"),j(u).triggerHandler("jqGridSubGridRowExpanded",[s+"_"+e,e]),j.jgrid.isFunction(u.p.subGridRowExpanded)?u.p.subGridRowExpanded.call(u,s+"_"+e,e):function(i){var e,s,d=j(i).attr("id"),r={nd_:(new Date).getTime()};if(r[u.p.prmNames.subgridid]=d,u.p.subGridModel[0]){if(u.p.subGridModel[0].params)for(s=0;s<u.p.subGridModel[0].params.length;s++)for(e=0;e<u.p.colModel.length;e++)u.p.colModel[e].name===u.p.subGridModel[0].params[s]&&(r[u.p.colModel[e].name]=j("td",i).eq(e).text().replace(/\&#160\;/gi,""));if(!u.grid.hDiv.loading)switch(u.grid.hDiv.loading=!0,j("#load_"+j.jgrid.jqID(u.p.id)).show(),u.p.subgridtype||(u.p.subgridtype=u.p.datatype),j.jgrid.isFunction(u.p.subgridtype)?u.p.subgridtype.call(u,r):u.p.subgridtype=u.p.subgridtype.toLowerCase(),u.p.subgridtype){case"xml":case"json":j.ajax(j.extend({type:u.p.mtype,url:j.jgrid.isFunction(u.p.subGridUrl)?u.p.subGridUrl.call(u,r):u.p.subGridUrl,dataType:u.p.subgridtype,data:j.jgrid.isFunction(u.p.serializeSubGridData)?u.p.serializeSubGridData.call(u,r):r,complete:function(i){"xml"===u.p.subgridtype?n(i.responseXML,d):a(j.jgrid.parse(i.responseText),d),i=null}},j.jgrid.ajaxOptions,u.p.ajaxSubgridOptions||{}))}}}(i)):t.show().removeClass("ui-sg-collapsed").addClass("ui-sg-expanded"),j(this).html("<a style='cursor:pointer;' class='ui-sghref'><span class='"+g.icon_base+" "+u.p.subGridOptions.minusicon+"'></span></a>").removeClass("sgcollapsed").addClass("sgexpanded"),u.p.subGridOptions.selectOnExpand&&j(u).jqGrid("setSelection",e)}else if(j(this).hasClass("sgexpanded")){if((r=!1!==(r=j(u).triggerHandler("jqGridSubGridRowColapsed",[s+"_"+e,e]))&&"stop"!==r)&&j.jgrid.isFunction(u.p.subGridRowColapsed)&&(r=u.p.subGridRowColapsed.call(u,s+"_"+e,e)),!1===r)return!1;!0===u.p.subGridOptions.reloadOnExpand?t.remove(".ui-subgrid"):t.hasClass("ui-subgrid")&&t.hide().addClass("ui-sg-collapsed").removeClass("ui-sg-expanded"),j(this).html("<a style='cursor:pointer;' class='ui-sghref'><span class='"+g.icon_base+" "+u.p.subGridOptions.plusicon+"'></span></a>").removeClass("sgexpanded").addClass("sgcollapsed"),u.p.subGridOptions.selectOnCollapse&&j(u).jqGrid("setSelection",e)}return!1})),h++;!0===u.p.subGridOptions.expandOnLoad&&(o=0,u.p.multiselect&&o++,u.p.rownumbers&&o++,j(u.rows).filter(".jqgrow").each(function(i,e){j(e.cells[o]).click()})),u.subGridXml=function(i,e){n(i,e)},u.subGridJson=function(i,e){a(i,e)}}})},expandSubGridRow:function(e){return this.each(function(){var i;(this.grid||e)&&!0===this.p.subGrid&&(!(i=j(this).jqGrid("getInd",e,!0))||(i=j("td.sgcollapsed",i)[0])&&j(i).trigger("click"))})},collapseSubGridRow:function(e){return this.each(function(){var i;(this.grid||e)&&!0===this.p.subGrid&&(!(i=j(this).jqGrid("getInd",e,!0))||(i=j("td.sgexpanded",i)[0])&&j(i).trigger("click"))})},toggleSubGridRow:function(s){return this.each(function(){var i,e;(this.grid||s)&&!0===this.p.subGrid&&(!(i=j(this).jqGrid("getInd",s,!0))||(e=(e=j("td.sgcollapsed",i)[0])||j("td.sgexpanded",i)[0])&&j(e).trigger("click"))})}})});