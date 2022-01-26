!function(r){"use strict";"function"==typeof define&&define.amd?define(["jquery","./grid.base"],r):r(jQuery)}(function(H){"use strict";H.jgrid.extend({groupingSetup:function(){return this.each(function(){var r,e,i,t=this,o=t.p.colModel,n=t.p.groupingView,s=H.jgrid.styleUI[t.p.styleUI||"jQueryUI"].grouping;if(null===n||"object"!=typeof n&&!H.jgrid.isFunction(n))t.p.grouping=!1;else if(n.plusicon||(n.plusicon=s.icon_plus),n.minusicon||(n.minusicon=s.icon_minus),n.groupField.length){for(void 0===n.visibiltyOnNextGrouping&&(n.visibiltyOnNextGrouping=[]),n.lastvalues=[],n._locgr||(n.groups=[]),n.counters=[],r=0;r<n.groupField.length;r++)n.groupOrder[r]||(n.groupOrder[r]="asc"),n.groupText[r]||(n.groupText[r]="{0}"),"boolean"!=typeof n.groupColumnShow[r]&&(n.groupColumnShow[r]=!0),"boolean"!=typeof n.groupSummary[r]&&(n.groupSummary[r]=!1),n.groupSummaryPos[r]||(n.groupSummaryPos[r]="footer"),!0===n.groupColumnShow[r]?(n.visibiltyOnNextGrouping[r]=!0,H(t).jqGrid("showCol",n.groupField[r])):(n.visibiltyOnNextGrouping[r]=H("#"+H.jgrid.jqID(t.p.id+"_"+n.groupField[r])).is(":visible"),H(t).jqGrid("hideCol",n.groupField[r]));for(n.summary=[],n.hideFirstGroupCol&&Array.isArray(n.formatDisplayField)&&!H.jgrid.isFunction(n.formatDisplayField[0])&&(n.formatDisplayField[0]=function(r){return r}),e=0,i=o.length;e<i;e++)n.hideFirstGroupCol&&(o[e].hidden||n.groupField[0]!==o[e].name||(o[e].formatter=function(){return""})),o[e].summaryType&&(o[e].summaryDivider?n.summary.push({nm:o[e].name,st:o[e].summaryType,v:"",sd:o[e].summaryDivider,vd:"",sr:o[e].summaryRound,srt:o[e].summaryRoundType||"round"}):n.summary.push({nm:o[e].name,st:o[e].summaryType,v:"",sr:o[e].summaryRound,srt:o[e].summaryRoundType||"round"}))}else t.p.grouping=!1})},groupingPrepare:function(d,l){return this.each(function(){for(var r,e,i,t=this.p.groupingView,o=this,n=function(){H.jgrid.isFunction(this.st)?this.v=this.st.call(o,this.v,this.nm,d):(this.v=H(o).jqGrid("groupingCalculations.handler",this.st,this.v,this.nm,this.sr,this.srt,d),"avg"===this.st.toLowerCase()&&this.sd&&(this.vd=H(o).jqGrid("groupingCalculations.handler",this.st,this.vd,this.sd,this.sr,this.srt,d)))},s=t.groupField.length,a=0,u=0;u<s;u++)r=t.groupField[u],i=t.displayField[u],e=d[r],null==(i=null==i?null:d[i])&&(i=e),void 0!==e&&(0===l?(t.groups.push({idx:u,dataIndex:r,value:e,displayValue:i,startRow:l,cnt:1,summary:[]}),t.lastvalues[u]=e,t.counters[u]={cnt:1,pos:t.groups.length-1,summary:H.extend(!0,[],t.summary)}):"object"==typeof e||(Array.isArray(t.isInTheSameGroup)&&H.jgrid.isFunction(t.isInTheSameGroup[u])?t.isInTheSameGroup[u].call(o,t.lastvalues[u],e,u,t):t.lastvalues[u]===e)?1===a?(t.groups.push({idx:u,dataIndex:r,value:e,displayValue:i,startRow:l,cnt:1,summary:[]}),t.lastvalues[u]=e,t.counters[u]={cnt:1,pos:t.groups.length-1,summary:H.extend(!0,[],t.summary)}):(t.counters[u].cnt+=1,t.groups[t.counters[u].pos].cnt=t.counters[u].cnt):(t.groups.push({idx:u,dataIndex:r,value:e,displayValue:i,startRow:l,cnt:1,summary:[]}),t.lastvalues[u]=e,a=1,t.counters[u]={cnt:1,pos:t.groups.length-1,summary:H.extend(!0,[],t.summary)}),H.each(t.counters[u].summary,n),t.groups[t.counters[u].pos].summary=t.counters[u].summary)}),this},groupingToggle:function(y){return this.each(function(){var r=this,e=r.p.groupingView,i=y.split("_"),t=parseInt(i[i.length-2],10);i.splice(i.length-2,2);function o(r){return 0<(r=H.map(r.split(" "),function(r){if(r.substring(0,u.length+1)===u+"_")return parseInt(r.substring(u.length+1),10)})).length?r[0]:void 0}var n,s,a,u=i.join("_"),d=e.minusicon,l=e.plusicon,p=H("#"+H.jgrid.jqID(y)),g=p.length?p[0].nextSibling:null,h=H("#"+H.jgrid.jqID(y)+" span.tree-wrap-"+r.p.direction),i=!1,c=!1,m=!!r.p.frozenColumns&&r.p.id+"_frozen",p=!!m&&H("#"+H.jgrid.jqID(y),"#"+H.jgrid.jqID(m)),f=p&&p.length?p[0].nextSibling:null;if(h.hasClass(d)){if(g)for(;g&&!(void 0!==(n=o(g.className))&&n<=t);)a=parseInt(H(g).attr("jqfootlevel"),10),(c=!isNaN(a)&&(e.showSummaryOnHide&&a<=t))||H(g).hide(),g=g.nextSibling,m&&(c||H(f).hide(),f=f.nextSibling);h.removeClass(d).addClass(l),i=!0}else{if(g)for(s=void 0;g;){if(n=o(g.className),void 0===s&&(s=void 0===n),c=H(g).hasClass("ui-subgrid")&&H(g).hasClass("ui-sg-collapsed"),void 0!==n){if(n<=t)break;n===t+1&&(c||(H(g).show().find(">td>span.tree-wrap-"+r.p.direction).removeClass(d).addClass(l),m&&H(f).show().find(">td>span.tree-wrap-"+r.p.direction).removeClass(d).addClass(l)))}else s&&(c||(H(g).show(),m&&H(f).show()));g=g.nextSibling,m&&(f=f.nextSibling)}h.removeClass(l).addClass(d)}H(r).triggerHandler("jqGridGroupingClickGroup",[y,i]),H.jgrid.isFunction(r.p.onClickGroup)&&r.p.onClickGroup.call(r,y,i)}),!1},groupingRender:function(G,F,S,I){return this.each(function(){var l,p,g,h,c=this,m=c.p.groupingView,f="",y=m.groupCollapse?m.plusicon:m.minusicon,v=[],j=m.groupField.length,x=H.jgrid.styleUI[c.p.styleUI||"jQueryUI"].common,y=y+" tree-wrap-"+c.p.direction;H.each(c.p.colModel,function(r,e){for(var i=0;i<j;i++)if(m.groupField[i]===e.name){v[i]=r;break}});var w=0;function q(r,e,i,t,o){for(var n,s,a,u=function(r,e,i){var t,o=!1;if(0===e)o=i[r];else{var n=i[r].idx;if(0===n)o=i[r];else for(t=r;0<=t;t--)if(i[t].idx===n-e){o=i[t];break}}return o}(r,e,i),d=c.p.colModel,l=u.cnt,p="",g=!1,h=t;h<F;h++)!d[h].hidden&&!g&&o?(s=o,g=!0):s="<td "+c.formatCol(h,1,"")+">&#160;</td>",H.each(u.summary,function(){if(this.nm===d[h].name){a=d[h].summaryTpl||"{0}","string"==typeof this.st&&"avg"===this.st.toLowerCase()&&(this.sd&&this.vd?this.v=this.v/this.vd:this.v&&0<l&&(this.v=this.v/l));try{this.groupCount=u.cnt,this.groupIndex=u.dataIndex,this.groupValue=u.value,n=c.formatter("",this.v,h,this)}catch(r){n=this.v}return s="<td "+c.formatCol(h,1,"")+">"+H.jgrid.template(a,n,u.cnt,u.dataIndex,u.displayValue)+"</td>",!1}}),p+=s;return p}var C,b=H.makeArray(m.groupSummary);b.reverse(),C=c.p.multiselect?' colspan="2"':"",H.each(m.groups,function(r,e){if(m._locgr&&!(e.startRow+e.cnt>(S-1)*I&&e.startRow<S*I))return!0;w++,g=c.p.id+"ghead_"+e.idx,p=g+"_"+r,l="<span style='cursor:pointer;margin-right:8px;margin-left:5px;' class='"+x.icon_base+" "+y+"' onclick=\"jQuery('#"+H.jgrid.jqID(c.p.id)+"').jqGrid('groupingToggle','"+p+"');return false;\"></span>";try{h=Array.isArray(m.formatDisplayField)&&H.jgrid.isFunction(m.formatDisplayField[e.idx])?m.formatDisplayField[e.idx].call(c,e.displayValue,e.value,c.p.colModel[v[e.idx]],e.idx,m):c.formatter(p,e.displayValue,v[e.idx],e.value)}catch(r){h=e.displayValue}var i="";if("string"!=typeof(i=H.jgrid.isFunction(m.groupText[e.idx])?m.groupText[e.idx].call(c,h,e.cnt,e.summary):H.jgrid.template(m.groupText[e.idx],h,e.cnt,e.summary))&&"number"!=typeof i&&(i=h),"header"===m.groupSummaryPos[e.idx]?(f+='<tr id="'+p+'"'+(m.groupCollapse&&0<e.idx?' style="display:none;" ':" ")+'role="row" class= "'+x.content+" jqgroup ui-row-"+c.p.direction+" "+g+'">',f+=q(r,0,m.groups,C?1:0,'<td style="padding-left:'+12*e.idx+'px;"'+C+">"+l+i+"</td>"),f+="</tr>"):f+='<tr id="'+p+'"'+(m.groupCollapse&&0<e.idx?' style="display:none;" ':" ")+'role="row" class= "'+x.content+" jqgroup ui-row-"+c.p.direction+" "+g+'"><td style="padding-left:'+12*e.idx+'px;" colspan="'+(!1===m.groupColumnShow[e.idx]?F-1:F)+'">'+l+i+"</td></tr>",j-1===e.idx){var t,o,n,s,a=m.groups[r+1],u=0,i=e.startRow,d=void 0!==a?a.startRow:m.groups[r].startRow+m.groups[r].cnt;for(m._locgr&&(u=(S-1)*I)>e.startRow&&(i=u),t=i;t<d&&G[t-u];t++)f+=G[t-u].join("");if("header"!==m.groupSummaryPos[e.idx]){if(void 0!==a){for(n=0;n<m.groupField.length&&a.dataIndex!==m.groupField[n];n++);w=m.groupField.length-n}for(o=0;o<w;o++)b[o]&&(s="",m.groupCollapse&&!m.showSummaryOnHide&&(s=' style="display:none;"'),f+="<tr"+s+' jqfootlevel="'+(e.idx-o)+'" role="row" class="'+x.content+" jqfoot ui-row-"+c.p.direction+'">',f+=q(r,o,m.groups,0,!1),f+="</tr>");w=n}}}),H("#"+H.jgrid.jqID(c.p.id)+" tbody").first().append(f),f=null})},groupingGroupBy:function(t,o){return this.each(function(){var r=this;"string"==typeof t&&(t=[t]);var e,i=r.p.groupingView;for(r.p.grouping=!0,i._locgr=!1,void 0===i.visibiltyOnNextGrouping&&(i.visibiltyOnNextGrouping=[]),e=0;e<i.groupField.length;e++)!i.groupColumnShow[e]&&i.visibiltyOnNextGrouping[e]&&H(r).jqGrid("showCol",i.groupField[e]);for(e=0;e<t.length;e++)i.visibiltyOnNextGrouping[e]=H("#"+H.jgrid.jqID(r.p.id)+"_"+H.jgrid.jqID(t[e])).is(":visible");r.p.groupingView=H.extend(r.p.groupingView,o||{}),i.groupField=t,H(r).trigger("reloadGrid")})},groupingRemove:function(t){return this.each(function(){var r=this;if(void 0===t&&(t=!0),!(r.p.grouping=!1)===t){for(var e=r.p.groupingView,i=0;i<e.groupField.length;i++)!e.groupColumnShow[i]&&e.visibiltyOnNextGrouping[i]&&H(r).jqGrid("showCol",e.groupField);H("#"+H.jgrid.jqID(r.p.id)+" tbody").first().find("tr.jqgroup, tr.jqfoot").remove(),H("#"+H.jgrid.jqID(r.p.id)+" tbody").first().find("tr.jqgrow:hidden").show()}else H(r).trigger("reloadGrid")})},groupingCalculations:{handler:function(r,e,i,t,o,n){var s={sum:function(){return parseFloat(e||0)+parseFloat(n[i]||0)},min:function(){return""===e?parseFloat(n[i]||0):Math.min(parseFloat(e),parseFloat(n[i]||0))},max:function(){return""===e?parseFloat(n[i]||0):Math.max(parseFloat(e),parseFloat(n[i]||0))},count:function(){return""===e&&(e=0),n.hasOwnProperty(i)?e+1:0},avg:function(){return s.sum()}};if(!s[r])throw"jqGrid Grouping No such method: "+r;r=s[r]();return null!=t&&(r="fixed"===o?r.toFixed(t):(t=Math.pow(10,t),Math.round(r*t)/t)),r}},setGroupHeaders:function(q){return q=H.extend({useColSpanStyle:!1,groupHeaders:[]},q||{}),this.each(function(){var r,e,i,t,o,n,s,a,u,d,l=this,p=0,g=l.p.colModel,h=g.length,c=l.grid.headers,m=H("table.ui-jqgrid-htable",l.grid.hDiv),f=m.children("thead").children("tr.ui-jqgrid-labels").last().addClass("jqg-second-row-header"),y=m.children("thead"),v=m.find(".jqg-first-row-header"),j=!1,x=H.jgrid.styleUI[l.p.styleUI||"jQueryUI"].base;l.p.groupHeader||(l.p.groupHeader=[]),l.p.groupHeader.push(q),l.p.groupHeaderOn=!0,void 0===v[0]?v=H("<tr>",{role:"row","aria-hidden":"true"}).addClass("jqg-first-row-header").css("height","auto"):v.empty();var w;for((H(document.activeElement).is("input")||H(document.activeElement).is("textarea"))&&(j=document.activeElement),H(l).prepend(y),H(l).prepend(y),i=H("<tr>",{role:"row"}).addClass("ui-jqgrid-labels jqg-third-row-header"),r=0;r<h;r++)if(t=c[r].el,o=H(t),e=g[r],u={height:"0px",width:c[r].width+"px",display:e.hidden?"none":""},H("<th>",{role:"gridcell"}).css(u).addClass("ui-first-th-"+l.p.direction).appendTo(v),t.style.width="",0<=(n=function(r,e){for(var i=e.length,t=0;t<i;t++)if(e[t].startColumnName===r)return t;return-1}(e.name,q.groupHeaders))){for(s=(u=q.groupHeaders[n]).numberOfColumns,w=u.titleText,u=u.className||"",n=a=0;n<s&&r+n<h;n++)g[r+n].hidden||a++;w=H("<th>").attr({role:"columnheader"}).addClass(x.headerBox+" ui-th-column-header ui-th-"+l.p.direction+" "+u).html(w),0<a&&w.attr("colspan",String(a)),l.p.headertitles&&w.attr("title",w.text()),0===a&&w.hide(),o.before(w),i.append(t),p=s-1}else 0===p?q.useColSpanStyle?(w=o.attr("rowspan")?parseInt(o.attr("rowspan"),10)+1:2,o.attr("rowspan",w)):(H("<th>",{role:"columnheader"}).addClass(x.headerBox+" ui-th-column-header ui-th-"+l.p.direction).css({display:e.hidden?"none":""}).insertBefore(o),i.append(t)):(i.append(t),p--);if((y=H(l).children("thead")).prepend(v),i.insertAfter(f),m.append(y),q.useColSpanStyle&&(m.find("span.ui-jqgrid-resize").each(function(){var r=H(this).parent();r.is(":visible")&&(this.style.cssText="height: "+r.height()+"px !important; cursor: col-resize;")}),m.find("div.ui-jqgrid-sortable").each(function(){var r=H(this),e=r.parent();e.is(":visible")&&e.is(":has(span.ui-jqgrid-resize)")&&r.css("top",(e.height()-r.outerHeight())/2-4+"px")})),d=y.find("tr.jqg-first-row-header"),H(l).on("jqGridResizeStop.setGroupHeaders",function(r,e,i){d.find("th").eq(i)[0].style.width=e+"px"}),j)try{H(j).focus()}catch(r){}""===H.jgrid.trim(H("tr.jqg-second-row-header th").eq(0).text())&&H("tr.jqg-second-row-header th").eq(0).prepend("&nbsp;")})},destroyGroupHeader:function(p){return void 0===p&&(p=!0),this.each(function(){var r,e,i,t,o,n,s=this,a=s.grid,u=H("table.ui-jqgrid-htable thead",a.hDiv),d=s.p.colModel,l=!1;if(a){for(s.p.frozenColumns&&(H(s).jqGrid("destroyFrozenColumns"),l=!0),H(this).off(".setGroupHeaders"),s.p.groupHeaderOn=!1,r=H("<tr>",{role:"row"}).addClass("ui-jqgrid-labels"),e=0,i=(t=a.headers).length;e<i;e++){n=d[e].hidden?"none":"",o=H(t[e].el).width(H("tr.jqg-first-row-header th",u).eq(e).width()).css("display",n);try{o.removeAttr("rowSpan")}catch(r){o.attr("rowSpan",1)}r.append(o),0<(n=o.children("span.ui-jqgrid-resize")).length&&(n[0].style.height=""),o.children("div")[0].style.top=""}H(u).children("tr.ui-jqgrid-labels").remove(),H(u).children("tr.jqg-first-row-header").remove(),H(u).prepend(r),!0===p&&H(s).jqGrid("setGridParam",{groupHeader:null}),l&&H(s).jqGrid("setFrozenColumns")}})},isGroupHeaderOn:function(){var r=this[0];return!0===r.p.groupHeaderOn&&r.p.groupHeader&&(Array.isArray(r.p.groupHeader)||H.jgrid.isFunction(r.p.groupHeader))},refreshGroupHeaders:function(){return this.each(function(){var r,e=this,i=H(e).jqGrid("isGroupHeaderOn");if(i&&(H(e).jqGrid("destroyGroupHeader",!1),r=H.extend([],e.p.groupHeader),e.p.groupHeader=null),i&&r)for(var t=0;t<r.length;t++)H(e).jqGrid("setGroupHeaders",r[t])})}})});