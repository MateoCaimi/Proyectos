var express = require("express");
var router = express.Router();
let controller = require("../controllers/usuarioController");
/* GET home page. */
router.post("/validarUsuario", function (req, res, next) {
  controller.validateUser(req, res, next);
});

module.exports = router;
