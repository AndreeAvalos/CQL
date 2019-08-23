Blockly.Blocks['sentencia_select'] = {
  init: function() {
    this.appendDummyInput();
    this.appendStatementInput("SELECT")
        .setCheck(null)
        .appendField("SELECT");
    this.appendValueInput("WHERE")
        .setCheck(null)
        .appendField("WHERE");
    this.setInputsInline(false);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['table'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["option","OPTIONNAME"], ["option","OPTIONNAME"], ["option","OPTIONNAME"]]), "NAME");
    this.setOutput(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['mayor'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField(">");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['menor'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField("<");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['igual'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField("=");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['and'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField("AND");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['or'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField("OR");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['diferente'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField("!=");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['mayor_igual'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField(">=");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['menor_igual'] = {
  init: function() {
    this.appendValueInput("primero")
        .setCheck(null);
    this.appendDummyInput()
        .appendField("<=");
    this.appendValueInput("segundo")
        .setCheck(null);
    this.setInputsInline(false);
    this.setOutput(true, null);
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['false'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("FALSE");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['true'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("TRUE");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['entero'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldNumber(2), "NAME");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['decimal'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldNumber(1.2, -Infinity, Infinity, 5), "NAME");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['fecha'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Fecha:")
        .appendField(new Blockly.FieldNumber(0, 1900, 2020), "year")
        .appendField(new Blockly.FieldNumber(0, 1, 12), "mes")
        .appendField(new Blockly.FieldNumber(0, 1, 31), "dia")
        .appendField(" (\"yyyy-mm-dd\")");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['opcion'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["option","OPTIONNAME"], ["option","OPTIONNAME"], ["option","OPTIONNAME"]]), "NAME")
        .appendField(new Blockly.FieldDropdown([["*","OPTIONNAME"], ["option","OPTIONNAME"], ["option","OPTIONNAME"]]), "COLUMNA");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['cadena'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldTextInput("\"EJEMPLO\""), "NAME");
    this.setOutput(true, null);
    this.setColour(345);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['sentencia_insert'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck(null)
        .appendField("INSERT INTO");
    this.appendValueInput("VALORES")
        .setCheck(null)
        .appendField("VALUES");
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['selector'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["option","OPTIONNAME"], ["option","OPTIONNAME"], ["option","OPTIONNAME"]]), "NAME")
        .appendField(new Blockly.FieldDropdown([["*","OPTIONNAME"], ["option","OPTIONNAME"], ["option","OPTIONNAME"]]), "TABLES");
    this.setPreviousStatement(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['lista'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Lista");
    this.appendStatementInput("NAME")
        .setCheck(null);
    this.setOutput(true, null);
    this.setColour(330);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['lista_valores'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck(null)
        .appendField("VALOR");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(330);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['sentencia_update'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck(null)
        .appendField("UPDATE ");
    this.appendValueInput("SET")
        .setCheck(null)
        .appendField("SET");
    this.appendValueInput("WHERE")
        .setCheck(null)
        .appendField("WHERE");
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['sentencia_delete'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck(null)
        .appendField("DELETE FROM");
    this.appendValueInput("NAME")
        .setCheck(null)
        .appendField("WHERE");
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};