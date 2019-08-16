/* parser generated by jison 0.4.18 */
/*
  Returns a Parser object of the following structure:

  Parser: {
    yy: {}
  }

  Parser.prototype: {
    yy: {},
    trace: function(),
    symbols_: {associative list: name ==> number},
    terminals_: {associative list: number ==> name},
    productions_: [...],
    performAction: function anonymous(yytext, yyleng, yylineno, yy, yystate, $$, _$),
    table: [...],
    defaultActions: {...},
    parseError: function(str, hash),
    parse: function(input),

    lexer: {
        EOF: 1,
        parseError: function(str, hash),
        setInput: function(input),
        input: function(),
        unput: function(str),
        more: function(),
        less: function(n),
        pastInput: function(),
        upcomingInput: function(),
        showPosition: function(),
        test_match: function(regex_match_array, rule_index),
        next: function(),
        lex: function(),
        begin: function(condition),
        popState: function(),
        _currentRules: function(),
        topState: function(),
        pushState: function(condition),

        options: {
            ranges: boolean           (optional: true ==> token location info will include a .range[] member)
            flex: boolean             (optional: true ==> flex-like lexing behaviour where the rules are tested exhaustively to find the longest match)
            backtrack_lexer: boolean  (optional: true ==> lexer regexes are tested in order and for each matching regex the action code is invoked; the lexer terminates the scan when a token is returned by the action code)
        },

        performAction: function(yy, yy_, $avoiding_name_collisions, YY_START),
        rules: [...],
        conditions: {associative list: name ==> set},
    }
  }


  token location info (@$, _$, etc.): {
    first_line: n,
    last_line: n,
    first_column: n,
    last_column: n,
    range: [start_number, end_number]       (where the numbers are indexes into the input string, regular zero-based)
  }


  the parseError function receives a 'hash' object with these members for lexer and parser errors: {
    text:        (matched text)
    token:       (the produced terminal token, if any)
    line:        (yylineno)
  }
  while parser (grammar) errors will also provide these members, i.e. parser errors deliver a superset of attributes: {
    loc:         (yylloc)
    expected:    (string describing the set of expected tokens)
    recoverable: (boolean: TRUE when the parser has a error recovery rule available for this particular error)
  }
*/
var gramatica = (function(){
var o=function(k,v,o,l){for(o=o||{},l=k.length;l--;o[k[l]]=v);return o},$V0=[1,12],$V1=[1,13],$V2=[2,5,15],$V3=[1,20],$V4=[1,35],$V5=[1,39],$V6=[1,41],$V7=[1,42],$V8=[1,43],$V9=[1,44],$Va=[1,56],$Vb=[15,26,27,28,29],$Vc=[1,72],$Vd=[1,126],$Ve=[1,193],$Vf=[1,202],$Vg=[1,208],$Vh=[1,266],$Vi=[1,269];
var parser = {trace: function trace () { },
yy: {},
symbols_: {"error":2,"S":3,"Instrucciones":4,"EOF":5,"Instruccion":6,"LOGIN":7,"LOGOUT":8,"QUERY":9,"DATA":10,"MESSAGE":11,"ERROR":12,"STRUC":13,"DATABASES":14,"CORIZQ":15,"MAS":16,"RLOGIN":17,"CORDER":18,"USER":19,"PASSWORD":20,"MENOS":21,"RESPUESTA":22,"RUSER":23,"VALOR":24,"RPASS":25,"CADENA":26,"IDENTIFICADOR":27,"ENTERO":28,"DECIMAL":29,"RSUCCESS":30,"RFAIL":31,"RLOGOUT":32,"RQUERY":33,"RDATA":34,"RMESSAGE":35,"VALORES":36,"RERROR":37,"LINEA":38,"COLUMNA":39,"TIPO":40,"DESCRIPCION":41,"RLINE":42,"RCOLUMN":43,"RTYPE":44,"RDESC":45,"RSTRUC":46,"RDATABASES":47,"DATABASES2":48,"DATABASE":49,"RDATABASE":50,"NAME":51,"TABLES":52,"TYPES":53,"PROCEDURES":54,"RNAME":55,"RTABLES":56,"TABLES2":57,"TABLE":58,"RTABLE":59,"COLUMNS":60,"RCOLUMNS":61,"COLUMNS2":62,"COLUMN":63,"RTYPES":64,"TYPES2":65,"TYPE":66,"ATTRIBUTES":67,"RATTRIBUTES":68,"ATTRIBUTES2":69,"ATTRIBUTE":70,"RATTRIBUTE":71,"RPROCEDURES":72,"PROCEDURES2":73,"PROCEDURE":74,"RPROCEDURE":75,"$accept":0,"$end":1},
terminals_: {2:"error",5:"EOF",15:"CORIZQ",16:"MAS",17:"RLOGIN",18:"CORDER",21:"MENOS",23:"RUSER",25:"RPASS",26:"CADENA",27:"IDENTIFICADOR",28:"ENTERO",29:"DECIMAL",30:"RSUCCESS",31:"RFAIL",32:"RLOGOUT",33:"RQUERY",34:"RDATA",35:"RMESSAGE",37:"RERROR",42:"RLINE",43:"RCOLUMN",44:"RTYPE",45:"RDESC",46:"RSTRUC",47:"RDATABASES",50:"RDATABASE",55:"RNAME",56:"RTABLES",59:"RTABLE",61:"RCOLUMNS",64:"RTYPES",68:"RATTRIBUTES",71:"RATTRIBUTE",72:"RPROCEDURES",75:"RPROCEDURE"},
productions_: [0,[3,2],[4,2],[4,1],[6,1],[6,1],[6,1],[6,1],[6,1],[6,1],[6,1],[6,1],[6,1],[7,10],[7,9],[19,9],[20,9],[24,1],[24,1],[24,1],[24,1],[22,3],[22,3],[8,9],[8,9],[9,10],[10,9],[11,9],[12,12],[38,9],[39,9],[40,9],[41,9],[13,9],[14,9],[48,2],[48,1],[49,12],[51,9],[52,9],[57,2],[57,1],[58,10],[58,9],[60,9],[62,2],[62,1],[63,9],[53,9],[65,2],[65,1],[66,10],[66,9],[67,9],[69,2],[69,1],[70,9],[54,9],[73,2],[73,1],[74,9],[36,2],[36,1]],
performAction: function anonymous(yytext, yyleng, yylineno, yy, yystate /* action[1] */, $$ /* vstack */, _$ /* lstack */) {
/* this == yyval */

var $0 = $$.length - 1;
switch (yystate) {
case 1:
return $$[$0-1]
break;
case 2:
 $$[$0-1].push($$[$0]); this.$ = $$[$0-1];
break;
case 3:
this.$ = [$$[$0]];
break;
case 4: case 5: case 7: case 8: case 9: case 11:
this.$ = $$[$0];
break;
case 12:
 console.error('Este es un error sintáctico: ' + yytext + ', en la linea: ' + this._$.first_line + ', en la columna: ' + this._$.first_column); 
break;
case 14:
this.$ = OBJETO_INSTRUCCION.nuevoLogin($$[$0-4]);
break;
case 17:
this.$ = OBJETO_INSTRUCCION.nuevoValor($$[$0],TIPO_VALOR.CADENA);
break;
case 18:
this.$ = OBJETO_INSTRUCCION.nuevoValor($$[$0],TIPO_VALOR.IDENTIFICADOR);
break;
case 19: case 20:
this.$ = OBJETO_INSTRUCCION.nuevoValor(Number($$[$0]),TIPO_VALOR.NUMERO);
break;
case 21:
this.$ = "true"
break;
case 22:
this.$ = "false"
break;
case 23:
this.$ = OBJETO_INSTRUCCION.nuevoLogout($$[$0-4]);
break;
case 26:
this.$ = OBJETO_INSTRUCCION.nuevoData($$[$0-4]);
break;
case 27:
this.$ = OBJETO_INSTRUCCION.nuevoMessage($$[$0-4]);
break;
case 28:
this.$ = OBJETO_INSTRUCCION.nuevoError($$[$0-7],$$[$0-6],$$[$0-5],$$[$0-4]);
break;
case 29: case 30: case 31: case 32: case 38: case 39:
this.$ = $$[$0-4];
break;
case 34:
this.$ = OBJETO_INSTRUCCION.nuevoDatabases($$[$0-4]);
break;
case 35: case 40: case 45: case 49: case 54: case 58: case 61:
$$[$0-1].push($$[$0]); this.$ = $$[$0-1];
break;
case 36: case 41: case 46: case 50: case 55: case 59: case 62:
 this.$ = [$$[$0]];
break;
case 37:
this.$ = OBJETO_INSTRUCCION.nuevoDatabase($$[$0-7],$$[$0-6],$$[$0-5],$$[$0-4]);
break;
case 42:
this.$ = OBJETO_INSTRUCCION.nuevoTable($$[$0-5],$$[$0-4]);
break;
case 43:
this.$ = OBJETO_INSTRUCCION.nuevoTable2($$[$0-4]);
break;
case 44: case 47: case 48: case 53: case 56: case 57:
 this.$ = $$[$0-4];
break;
case 51:
this.$ = OBJETO_INSTRUCCION.nuevoType($$[$0-5],$$[$0-4]);
break;
case 52:
this.$ = OBJETO_INSTRUCCION.nuevoType2($$[$0-4]);
break;
}
},
table: [{2:$V0,3:1,4:2,6:3,7:4,8:5,9:6,10:7,11:8,12:9,13:10,14:11,15:$V1},{1:[3]},{2:$V0,5:[1,14],6:15,7:4,8:5,9:6,10:7,11:8,12:9,13:10,14:11,15:$V1},o($V2,[2,3]),o($V2,[2,4]),o($V2,[2,5]),o($V2,[2,6]),o($V2,[2,7]),o($V2,[2,8]),o($V2,[2,9]),o($V2,[2,10]),o($V2,[2,11]),o($V2,[2,12]),{16:[1,16]},{1:[2,1]},o($V2,[2,2]),{17:[1,17],32:[1,18],33:[1,19],34:$V3,35:[1,21],37:[1,22],46:[1,23],47:[1,24]},{18:[1,25]},{18:[1,26]},{18:[1,27]},{18:[1,28]},{18:[1,29]},{18:[1,30]},{18:[1,31]},{18:[1,32]},{15:$V4,19:33,22:34},{15:$V4,19:37,22:36},{15:$V5,19:38},{24:40,26:$V6,27:$V7,28:$V8,29:$V9},{24:46,26:$V6,27:$V7,28:$V8,29:$V9,36:45},{15:[1,48],38:47},{15:$V5,19:49},{15:[1,52],48:50,49:51},{15:[1,54],20:53},{15:[1,55]},{16:$Va,30:[1,57],31:[1,58]},{15:[1,59]},{15:[1,60]},{10:61,15:[1,62]},{16:$Va},{15:[1,63]},o($Vb,[2,17]),o($Vb,[2,18]),o($Vb,[2,19]),o($Vb,[2,20]),{15:[1,64],24:65,26:$V6,27:$V7,28:$V8,29:$V9},o($Vb,[2,62]),{15:[1,67],39:66},{16:[1,68]},{15:[1,69]},{15:[1,70],49:71},{15:[2,36]},{16:$Vc},{15:[1,73]},{16:[1,74]},{21:[1,75]},{23:[1,76]},{18:[1,77]},{18:[1,78]},{21:[1,79]},{21:[1,80]},{15:[1,81]},{16:[1,82]},{21:[1,83]},{21:[1,84]},o($Vb,[2,61]),{15:[1,86],40:85},{16:[1,87]},{42:[1,88]},{21:[1,89]},{16:$Vc,21:[1,90]},{15:[2,35]},{50:[1,91]},{21:[1,92]},{25:[1,93]},{17:[1,94]},{18:[1,95]},{15:[2,21]},{15:[2,22]},{32:[1,96]},{32:[1,97]},{21:[1,98]},{34:$V3},{34:[1,99]},{35:[1,100]},{15:[1,102],41:101},{16:[1,103]},{43:[1,104]},{18:[1,105]},{46:[1,106]},{47:[1,107]},{18:[1,108]},{17:[1,109]},{18:[1,110]},{18:[1,111]},{24:112,26:$V6,27:$V7,28:$V8,29:$V9},{18:[1,113]},{18:[1,114]},{33:[1,115]},{18:[1,116]},{18:[1,117]},{15:[1,118]},{16:[1,119]},{44:[1,120]},{18:[1,121]},{24:122,26:$V6,27:$V7,28:$V8,29:$V9},{18:[1,123]},{18:[1,124]},{15:$Vd,51:125},{18:[1,127]},{24:128,26:$V6,27:$V7,28:$V8,29:$V9},o($V2,[2,14]),{15:[1,129]},o($V2,[2,23]),o($V2,[2,24]),{18:[1,130]},o($V2,[2,26]),o($V2,[2,27]),{21:[1,131]},{45:[1,132]},{18:[1,133]},{24:134,26:$V6,27:$V7,28:$V8,29:$V9},{15:[1,135]},o($V2,[2,33]),o($V2,[2,34]),{15:[1,137],52:136},{16:[1,138]},o($V2,[2,13]),{15:[1,139]},{21:[1,140]},o($V2,[2,25]),{37:[1,141]},{18:[1,142]},{24:143,26:$V6,27:$V7,28:$V8,29:$V9},{15:[1,144]},{21:[1,145]},{15:[1,147],53:146},{16:[1,148]},{55:[1,149]},{21:[1,150]},{23:[1,151]},{18:[1,152]},{24:153,26:$V6,27:$V7,28:$V8,29:$V9},{15:[1,154]},{21:[1,155]},{42:[1,156]},{15:[1,158],54:157},{16:[1,159]},{56:[1,160]},{18:[1,161]},{25:[1,162]},{18:[1,163]},o($V2,[2,28]),{15:[1,164]},{21:[1,165]},{43:[1,166]},{18:[1,167]},{15:[1,168]},{16:[1,169]},{64:[1,170]},{18:[1,171]},{24:172,26:$V6,27:$V7,28:$V8,29:$V9},{18:[1,173]},{15:[2,15]},{21:[1,174]},{44:[1,175]},{18:[1,176]},{15:[2,29]},{21:[1,177]},{72:[1,178]},{18:[1,179]},{15:[1,182],57:180,58:181},{15:[1,183]},{15:[2,16]},{45:[1,184]},{18:[1,185]},{15:[2,30]},{50:[1,186]},{18:[1,187]},{15:[1,190],65:188,66:189},{15:[1,191],58:192},{15:[2,41]},{16:$Ve},{21:[1,194]},{18:[1,195]},{15:[2,31]},{18:[1,196]},{15:[1,199],73:197,74:198},{15:[1,200],66:201},{15:[2,50]},{16:$Vf},{16:$Ve,21:[1,203]},{15:[2,40]},{59:[1,204]},{55:[1,205]},{15:[2,32]},{15:[2,37]},{15:[1,206],74:207},{15:[2,59]},{16:$Vg},{16:$Vf,21:[1,209]},{15:[2,49]},{44:[1,210]},{56:[1,211]},{18:[1,212]},{18:[1,213]},{16:$Vg,21:[1,214]},{15:[2,58]},{75:[1,215]},{64:[1,216]},{18:[1,217]},{18:[1,218]},{15:$Vd,24:46,26:$V6,27:$V7,28:$V8,29:$V9,36:220,51:219},{15:[2,38]},{72:[1,221]},{18:[1,222]},{18:[1,223]},{15:$Vd,24:46,26:$V6,27:$V7,28:$V8,29:$V9,36:225,51:224},{15:[2,39]},{15:[1,227],60:226},{15:[1,228],24:65,26:$V6,27:$V7,28:$V8,29:$V9},{18:[1,229]},{15:$Vd,51:230},{15:[2,48]},{15:[1,232],67:231},{15:[1,233],24:65,26:$V6,27:$V7,28:$V8,29:$V9},{15:[1,234]},{16:[1,235]},{21:[1,236]},{15:[2,57]},{15:[1,237]},{15:[1,238]},{16:[1,239]},{21:[1,240]},{21:[1,241]},{61:[1,242]},{59:[1,243]},{21:[1,244]},{21:[1,245]},{68:[1,246]},{44:[1,247]},{59:[1,248]},{18:[1,249]},{18:[1,250]},{75:[1,251]},{44:[1,252]},{18:[1,253]},{18:[1,254]},{18:[1,255]},{15:[1,258],62:256,63:257},{15:[2,43]},{18:[1,259]},{18:[1,260]},{15:[1,263],69:261,70:262},{15:[2,52]},{15:[2,42]},{15:[1,264],63:265},{15:[2,46]},{16:$Vh},{15:[2,60]},{15:[2,51]},{15:[1,267],70:268},{15:[2,55]},{16:$Vi},{16:$Vh,21:[1,270]},{15:[2,45]},{43:[1,271]},{16:$Vi,21:[1,272]},{15:[2,54]},{71:[1,273]},{61:[1,274]},{18:[1,275]},{68:[1,276]},{18:[1,277]},{18:[1,278]},{15:$Vd,51:279},{18:[1,280]},{15:$Vd,51:281},{15:[2,44]},{15:[1,282]},{15:[2,53]},{15:[1,283]},{21:[1,284]},{21:[1,285]},{43:[1,286]},{71:[1,287]},{18:[1,288]},{18:[1,289]},{15:[2,47]},{15:[2,56]}],
defaultActions: {14:[2,1],51:[2,36],71:[2,35],77:[2,21],78:[2,22],163:[2,15],167:[2,29],173:[2,16],176:[2,30],181:[2,41],185:[2,31],189:[2,50],192:[2,40],195:[2,32],196:[2,37],198:[2,59],201:[2,49],207:[2,58],213:[2,38],218:[2,39],223:[2,48],229:[2,57],250:[2,43],254:[2,52],255:[2,42],257:[2,46],259:[2,60],260:[2,51],262:[2,55],265:[2,45],268:[2,54],278:[2,44],280:[2,53],288:[2,47],289:[2,56]},
parseError: function parseError (str, hash) {
    if (hash.recoverable) {
        this.trace(str);
    } else {
        var error = new Error(str);
        error.hash = hash;
        throw error;
    }
},
parse: function parse (input) {
    var self = this,
        stack = [0],
        tstack = [], // token stack
        vstack = [null], // semantic value stack
        lstack = [], // location stack
        table = this.table,
        yytext = '',
        yylineno = 0,
        yyleng = 0,
        recovering = 0,
        TERROR = 2,
        EOF = 1;

    var args = lstack.slice.call(arguments, 1);

    //this.reductionCount = this.shiftCount = 0;

    var lexer = Object.create(this.lexer);
    var sharedState = { yy: {} };
    // copy state
    for (var k in this.yy) {
      if (Object.prototype.hasOwnProperty.call(this.yy, k)) {
        sharedState.yy[k] = this.yy[k];
      }
    }

    lexer.setInput(input, sharedState.yy);
    sharedState.yy.lexer = lexer;
    sharedState.yy.parser = this;
    if (typeof lexer.yylloc == 'undefined') {
        lexer.yylloc = {};
    }
    var yyloc = lexer.yylloc;
    lstack.push(yyloc);

    var ranges = lexer.options && lexer.options.ranges;

    if (typeof sharedState.yy.parseError === 'function') {
        this.parseError = sharedState.yy.parseError;
    } else {
        this.parseError = Object.getPrototypeOf(this).parseError;
    }

    function popStack (n) {
        stack.length = stack.length - 2 * n;
        vstack.length = vstack.length - n;
        lstack.length = lstack.length - n;
    }

_token_stack:
    var lex = function () {
        var token;
        token = lexer.lex() || EOF;
        // if token isn't its numeric value, convert
        if (typeof token !== 'number') {
            token = self.symbols_[token] || token;
        }
        return token;
    }

    var symbol, preErrorSymbol, state, action, a, r, yyval = {}, p, len, newState, expected;
    while (true) {
        // retreive state number from top of stack
        state = stack[stack.length - 1];

        // use default actions if available
        if (this.defaultActions[state]) {
            action = this.defaultActions[state];
        } else {
            if (symbol === null || typeof symbol == 'undefined') {
                symbol = lex();
            }
            // read action for current state and first input
            action = table[state] && table[state][symbol];
        }

_handle_error:
        // handle parse error
        if (typeof action === 'undefined' || !action.length || !action[0]) {
            var error_rule_depth;
            var errStr = '';

            // Return the rule stack depth where the nearest error rule can be found.
            // Return FALSE when no error recovery rule was found.
            function locateNearestErrorRecoveryRule(state) {
                var stack_probe = stack.length - 1;
                var depth = 0;

                // try to recover from error
                for(;;) {
                    // check for error recovery rule in this state
                    if ((TERROR.toString()) in table[state]) {
                        return depth;
                    }
                    if (state === 0 || stack_probe < 2) {
                        return false; // No suitable error recovery rule available.
                    }
                    stack_probe -= 2; // popStack(1): [symbol, action]
                    state = stack[stack_probe];
                    ++depth;
                }
            }

            if (!recovering) {
                // first see if there's any chance at hitting an error recovery rule:
                error_rule_depth = locateNearestErrorRecoveryRule(state);

                // Report error
                expected = [];
                for (p in table[state]) {
                    if (this.terminals_[p] && p > TERROR) {
                        expected.push("'"+this.terminals_[p]+"'");
                    }
                }
                if (lexer.showPosition) {
                    errStr = 'Parse error on line '+(yylineno+1)+":\n"+lexer.showPosition()+"\nExpecting "+expected.join(', ') + ", got '" + (this.terminals_[symbol] || symbol)+ "'";
                } else {
                    errStr = 'Parse error on line '+(yylineno+1)+": Unexpected " +
                                  (symbol == EOF ? "end of input" :
                                              ("'"+(this.terminals_[symbol] || symbol)+"'"));
                }
                this.parseError(errStr, {
                    text: lexer.match,
                    token: this.terminals_[symbol] || symbol,
                    line: lexer.yylineno,
                    loc: yyloc,
                    expected: expected,
                    recoverable: (error_rule_depth !== false)
                });
            } else if (preErrorSymbol !== EOF) {
                error_rule_depth = locateNearestErrorRecoveryRule(state);
            }

            // just recovered from another error
            if (recovering == 3) {
                if (symbol === EOF || preErrorSymbol === EOF) {
                    throw new Error(errStr || 'Parsing halted while starting to recover from another error.');
                }

                // discard current lookahead and grab another
                yyleng = lexer.yyleng;
                yytext = lexer.yytext;
                yylineno = lexer.yylineno;
                yyloc = lexer.yylloc;
                symbol = lex();
            }

            // try to recover from error
            if (error_rule_depth === false) {
                throw new Error(errStr || 'Parsing halted. No suitable error recovery rule available.');
            }
            popStack(error_rule_depth);

            preErrorSymbol = (symbol == TERROR ? null : symbol); // save the lookahead token
            symbol = TERROR;         // insert generic error symbol as new lookahead
            state = stack[stack.length-1];
            action = table[state] && table[state][TERROR];
            recovering = 3; // allow 3 real symbols to be shifted before reporting a new error
        }

        // this shouldn't happen, unless resolve defaults are off
        if (action[0] instanceof Array && action.length > 1) {
            throw new Error('Parse Error: multiple actions possible at state: '+state+', token: '+symbol);
        }

        switch (action[0]) {
            case 1: // shift
                //this.shiftCount++;

                stack.push(symbol);
                vstack.push(lexer.yytext);
                lstack.push(lexer.yylloc);
                stack.push(action[1]); // push state
                symbol = null;
                if (!preErrorSymbol) { // normal execution/no error
                    yyleng = lexer.yyleng;
                    yytext = lexer.yytext;
                    yylineno = lexer.yylineno;
                    yyloc = lexer.yylloc;
                    if (recovering > 0) {
                        recovering--;
                    }
                } else {
                    // error just occurred, resume old lookahead f/ before error
                    symbol = preErrorSymbol;
                    preErrorSymbol = null;
                }
                break;

            case 2:
                // reduce
                //this.reductionCount++;

                len = this.productions_[action[1]][1];

                // perform semantic action
                yyval.$ = vstack[vstack.length-len]; // default to $$ = $1
                // default location, uses first token for firsts, last for lasts
                yyval._$ = {
                    first_line: lstack[lstack.length-(len||1)].first_line,
                    last_line: lstack[lstack.length-1].last_line,
                    first_column: lstack[lstack.length-(len||1)].first_column,
                    last_column: lstack[lstack.length-1].last_column
                };
                if (ranges) {
                  yyval._$.range = [lstack[lstack.length-(len||1)].range[0], lstack[lstack.length-1].range[1]];
                }
                r = this.performAction.apply(yyval, [yytext, yyleng, yylineno, sharedState.yy, action[1], vstack, lstack].concat(args));

                if (typeof r !== 'undefined') {
                    return r;
                }

                // pop off stack
                if (len) {
                    stack = stack.slice(0,-1*len*2);
                    vstack = vstack.slice(0, -1*len);
                    lstack = lstack.slice(0, -1*len);
                }

                stack.push(this.productions_[action[1]][0]);    // push nonterminal (reduce)
                vstack.push(yyval.$);
                lstack.push(yyval._$);
                // goto new state = table[STATE][NONTERMINAL]
                newState = table[stack[stack.length-2]][stack[stack.length-1]];
                stack.push(newState);
                break;

            case 3:
                // accept
                return true;
        }

    }

    return true;
}};

	const TIPO_INSTRUCCION	    = require('./instrucciones').TIPO_INSTRUCCION;
	const TIPO_VALOR 		    = require('./instrucciones').TIPO_VALOR;
	const OBJETO_INSTRUCCION    = require('./instrucciones').OBJETO_INSTRUCCION;
    const OBJETO_DATABASE    = require('./instrucciones').OBJETO_DATABASE;
/* generated by jison-lex 0.3.4 */
var lexer = (function(){
var lexer = ({

EOF:1,

parseError:function parseError(str, hash) {
        if (this.yy.parser) {
            this.yy.parser.parseError(str, hash);
        } else {
            throw new Error(str);
        }
    },

// resets the lexer, sets new input
setInput:function (input, yy) {
        this.yy = yy || this.yy || {};
        this._input = input;
        this._more = this._backtrack = this.done = false;
        this.yylineno = this.yyleng = 0;
        this.yytext = this.matched = this.match = '';
        this.conditionStack = ['INITIAL'];
        this.yylloc = {
            first_line: 1,
            first_column: 0,
            last_line: 1,
            last_column: 0
        };
        if (this.options.ranges) {
            this.yylloc.range = [0,0];
        }
        this.offset = 0;
        return this;
    },

// consumes and returns one char from the input
input:function () {
        var ch = this._input[0];
        this.yytext += ch;
        this.yyleng++;
        this.offset++;
        this.match += ch;
        this.matched += ch;
        var lines = ch.match(/(?:\r\n?|\n).*/g);
        if (lines) {
            this.yylineno++;
            this.yylloc.last_line++;
        } else {
            this.yylloc.last_column++;
        }
        if (this.options.ranges) {
            this.yylloc.range[1]++;
        }

        this._input = this._input.slice(1);
        return ch;
    },

// unshifts one char (or a string) into the input
unput:function (ch) {
        var len = ch.length;
        var lines = ch.split(/(?:\r\n?|\n)/g);

        this._input = ch + this._input;
        this.yytext = this.yytext.substr(0, this.yytext.length - len);
        //this.yyleng -= len;
        this.offset -= len;
        var oldLines = this.match.split(/(?:\r\n?|\n)/g);
        this.match = this.match.substr(0, this.match.length - 1);
        this.matched = this.matched.substr(0, this.matched.length - 1);

        if (lines.length - 1) {
            this.yylineno -= lines.length - 1;
        }
        var r = this.yylloc.range;

        this.yylloc = {
            first_line: this.yylloc.first_line,
            last_line: this.yylineno + 1,
            first_column: this.yylloc.first_column,
            last_column: lines ?
                (lines.length === oldLines.length ? this.yylloc.first_column : 0)
                 + oldLines[oldLines.length - lines.length].length - lines[0].length :
              this.yylloc.first_column - len
        };

        if (this.options.ranges) {
            this.yylloc.range = [r[0], r[0] + this.yyleng - len];
        }
        this.yyleng = this.yytext.length;
        return this;
    },

// When called from action, caches matched text and appends it on next action
more:function () {
        this._more = true;
        return this;
    },

// When called from action, signals the lexer that this rule fails to match the input, so the next matching rule (regex) should be tested instead.
reject:function () {
        if (this.options.backtrack_lexer) {
            this._backtrack = true;
        } else {
            return this.parseError('Lexical error on line ' + (this.yylineno + 1) + '. You can only invoke reject() in the lexer when the lexer is of the backtracking persuasion (options.backtrack_lexer = true).\n' + this.showPosition(), {
                text: "",
                token: null,
                line: this.yylineno
            });

        }
        return this;
    },

// retain first n characters of the match
less:function (n) {
        this.unput(this.match.slice(n));
    },

// displays already matched input, i.e. for error messages
pastInput:function () {
        var past = this.matched.substr(0, this.matched.length - this.match.length);
        return (past.length > 20 ? '...':'') + past.substr(-20).replace(/\n/g, "");
    },

// displays upcoming input, i.e. for error messages
upcomingInput:function () {
        var next = this.match;
        if (next.length < 20) {
            next += this._input.substr(0, 20-next.length);
        }
        return (next.substr(0,20) + (next.length > 20 ? '...' : '')).replace(/\n/g, "");
    },

// displays the character position where the lexing error occurred, i.e. for error messages
showPosition:function () {
        var pre = this.pastInput();
        var c = new Array(pre.length + 1).join("-");
        return pre + this.upcomingInput() + "\n" + c + "^";
    },

// test the lexed token: return FALSE when not a match, otherwise return token
test_match:function(match, indexed_rule) {
        var token,
            lines,
            backup;

        if (this.options.backtrack_lexer) {
            // save context
            backup = {
                yylineno: this.yylineno,
                yylloc: {
                    first_line: this.yylloc.first_line,
                    last_line: this.last_line,
                    first_column: this.yylloc.first_column,
                    last_column: this.yylloc.last_column
                },
                yytext: this.yytext,
                match: this.match,
                matches: this.matches,
                matched: this.matched,
                yyleng: this.yyleng,
                offset: this.offset,
                _more: this._more,
                _input: this._input,
                yy: this.yy,
                conditionStack: this.conditionStack.slice(0),
                done: this.done
            };
            if (this.options.ranges) {
                backup.yylloc.range = this.yylloc.range.slice(0);
            }
        }

        lines = match[0].match(/(?:\r\n?|\n).*/g);
        if (lines) {
            this.yylineno += lines.length;
        }
        this.yylloc = {
            first_line: this.yylloc.last_line,
            last_line: this.yylineno + 1,
            first_column: this.yylloc.last_column,
            last_column: lines ?
                         lines[lines.length - 1].length - lines[lines.length - 1].match(/\r?\n?/)[0].length :
                         this.yylloc.last_column + match[0].length
        };
        this.yytext += match[0];
        this.match += match[0];
        this.matches = match;
        this.yyleng = this.yytext.length;
        if (this.options.ranges) {
            this.yylloc.range = [this.offset, this.offset += this.yyleng];
        }
        this._more = false;
        this._backtrack = false;
        this._input = this._input.slice(match[0].length);
        this.matched += match[0];
        token = this.performAction.call(this, this.yy, this, indexed_rule, this.conditionStack[this.conditionStack.length - 1]);
        if (this.done && this._input) {
            this.done = false;
        }
        if (token) {
            return token;
        } else if (this._backtrack) {
            // recover context
            for (var k in backup) {
                this[k] = backup[k];
            }
            return false; // rule action called reject() implying the next rule should be tested instead.
        }
        return false;
    },

// return next match in input
next:function () {
        if (this.done) {
            return this.EOF;
        }
        if (!this._input) {
            this.done = true;
        }

        var token,
            match,
            tempMatch,
            index;
        if (!this._more) {
            this.yytext = '';
            this.match = '';
        }
        var rules = this._currentRules();
        for (var i = 0; i < rules.length; i++) {
            tempMatch = this._input.match(this.rules[rules[i]]);
            if (tempMatch && (!match || tempMatch[0].length > match[0].length)) {
                match = tempMatch;
                index = i;
                if (this.options.backtrack_lexer) {
                    token = this.test_match(tempMatch, rules[i]);
                    if (token !== false) {
                        return token;
                    } else if (this._backtrack) {
                        match = false;
                        continue; // rule action called reject() implying a rule MISmatch.
                    } else {
                        // else: this is a lexer rule which consumes input without producing a token (e.g. whitespace)
                        return false;
                    }
                } else if (!this.options.flex) {
                    break;
                }
            }
        }
        if (match) {
            token = this.test_match(match, rules[index]);
            if (token !== false) {
                return token;
            }
            // else: this is a lexer rule which consumes input without producing a token (e.g. whitespace)
            return false;
        }
        if (this._input === "") {
            return this.EOF;
        } else {
            return this.parseError('Lexical error on line ' + (this.yylineno + 1) + '. Unrecognized text.\n' + this.showPosition(), {
                text: "",
                token: null,
                line: this.yylineno
            });
        }
    },

// return next match that has a token
lex:function lex () {
        var r = this.next();
        if (r) {
            return r;
        } else {
            return this.lex();
        }
    },

// activates a new lexer condition state (pushes the new lexer condition state onto the condition stack)
begin:function begin (condition) {
        this.conditionStack.push(condition);
    },

// pop the previously active lexer condition state off the condition stack
popState:function popState () {
        var n = this.conditionStack.length - 1;
        if (n > 0) {
            return this.conditionStack.pop();
        } else {
            return this.conditionStack[0];
        }
    },

// produce the lexer rule set which is active for the currently active lexer condition state
_currentRules:function _currentRules () {
        if (this.conditionStack.length && this.conditionStack[this.conditionStack.length - 1]) {
            return this.conditions[this.conditionStack[this.conditionStack.length - 1]].rules;
        } else {
            return this.conditions["INITIAL"].rules;
        }
    },

// return the currently active lexer condition state; when an index argument is provided it produces the N-th previous condition state, if available
topState:function topState (n) {
        n = this.conditionStack.length - 1 - Math.abs(n || 0);
        if (n >= 0) {
            return this.conditionStack[n];
        } else {
            return "INITIAL";
        }
    },

// alias for begin(condition)
pushState:function pushState (condition) {
        this.begin(condition);
    },

// return the number of states currently on the stack
stateStackSize:function stateStackSize() {
        return this.conditionStack.length;
    },
options: {"case-insensitive":true},
performAction: function anonymous(yy,yy_,$avoiding_name_collisions,YY_START) {
var YYSTATE=YY_START;
switch($avoiding_name_collisions) {
case 0:// se ignoran espacios en blanco
break;
case 1:// comentario simple línea
break;
case 2:// comentario multiple líneas
break;
case 3:return 17;
break;
case 4:return 30;
break;
case 5:return 31;
break;
case 6:return 32;
break;
case 7:return 23
break;
case 8:return 25;
break;
case 9:return 33;
break;
case 10:return 34;
break;
case 11:return 35;
break;
case 12:return 37;
break;
case 13:return 42;
break;
case 14:return 43;
break;
case 15:return 44;
break;
case 16:return 45;
break;
case 17:return 46;
break;
case 18:return 47;
break;
case 19:return 50;
break;
case 20:return 55;
break;
case 21:return 56;
break;
case 22:return 59;
break;
case 23:return 61;
break;
case 24:return 64;
break;
case 25:return 44;
break;
case 26:return 68;
break;
case 27:return 71;
break;
case 28:return 72;
break;
case 29:return 75;
break;
case 30:return 15;
break;
case 31:return 18;
break;
case 32:return 16;
break;
case 33:return 21;
break;
case 34: yy_.yytext = yy_.yytext.substr(1,yy_.yyleng-2); return 26; 
break;
case 35:return 29;
break;
case 36:return 28;
break;
case 37:return 27;
break;
case 38:return 5;
break;
case 39: console.error('Este es un error léxico: ' + yy_.yytext + ', en la linea: ' + yy_.yylloc.first_line + ', en la columna: ' + yy_.yylloc.first_column); 
break;
}
},
rules: [/^(?:\s+)/i,/^(?:\/\/.*)/i,/^(?:[\/][*][^*]*[*]+([^\/*][^*]*[*]+)*[\/])/i,/^(?:LOGIN\b)/i,/^(?:SUCCESS\b)/i,/^(?:FAIL\b)/i,/^(?:LOGOUT\b)/i,/^(?:USER\b)/i,/^(?:PASS\b)/i,/^(?:QUERY\b)/i,/^(?:DATA\b)/i,/^(?:MESSAGE\b)/i,/^(?:ERROR\b)/i,/^(?:LINE\b)/i,/^(?:COLUMN\b)/i,/^(?:TYPE\b)/i,/^(?:DESC\b)/i,/^(?:STRUC\b)/i,/^(?:DATABASES\b)/i,/^(?:DATABASE\b)/i,/^(?:NAME\b)/i,/^(?:TABLES\b)/i,/^(?:TABLE\b)/i,/^(?:COLUMNS\b)/i,/^(?:TYPES\b)/i,/^(?:TYPE\b)/i,/^(?:ATTRIBUTES\b)/i,/^(?:ATTRIBUTE\b)/i,/^(?:PROCEDURES\b)/i,/^(?:PROCEDURE\b)/i,/^(?:\[)/i,/^(?:\])/i,/^(?:\+)/i,/^(?:-)/i,/^(?:"(\\.|[^\\"])*")/i,/^(?:[0-9]+(\.[0-9]+)?\b)/i,/^(?:[0-9]+\b)/i,/^(?:([a-zA-Z])[a-zA-Z0-9_]*)/i,/^(?:$)/i,/^(?:.)/i],
conditions: {"INITIAL":{"rules":[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39],"inclusive":true}}
});
return lexer;
})();
parser.lexer = lexer;
function Parser () {
  this.yy = {};
}
Parser.prototype = parser;parser.Parser = Parser;
return new Parser;
})();


if (typeof require !== 'undefined' && typeof exports !== 'undefined') {
exports.parser = gramatica;
exports.Parser = gramatica.Parser;
exports.parse = function () { return gramatica.parse.apply(gramatica, arguments); };
exports.main = function commonjsMain (args) {
    if (!args[1]) {
        console.log('Usage: '+args[0]+' FILE');
        process.exit(1);
    }
    var source = require('fs').readFileSync(require('path').normalize(args[1]), "utf8");
    return exports.parser.parse(source);
};
if (typeof module !== 'undefined' && require.main === module) {
  exports.main(process.argv.slice(1));
}
}