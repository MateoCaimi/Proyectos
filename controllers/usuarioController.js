var express = require("express");
const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");
const jwt = require("jsonwebtoken");
const fs = require("fs");

const RSA_PRIVATE_KEY = fs.readFileSync("private.key");

exports.validateUser = asyncHandler(async (req, res, next) => {
  const username = req.body.username;
  const password = req.body.password;

  const data = {
    username,
    password,
  };

  const response = {
    resultado: {
      statusCode: "",
      statusText: "",
    },
    user: {
      idUsuario: "",
      username: "",
      token: "",
    },
  };
  if (username && password) {
    try {
      const result = await dataBase.ejecutarConsulta(
        `SELECT * FROM potencia_vial.Usuario where Usuario ="${username}" and Contraseña = "${password}"`
      );
      if (result.length === 0) {
        response.resultado.statusCode = "404";
        response.resultado.statusText = "User Not Found";

        res.status(200).json(response);
      } else {
        response.resultado.statusCode = "200";
        response.resultado.statusText = "OK";

        response.user.idUsuario = result[0].Usuario;
        response.user.username = result[0].Contraseña;
        const idUser = result[0].Usuario.toString();
        const token = generateJWT(idUser);
        response.user.token = token;

        res.cookie("SESSIONID", jwt, { httpOnly: true, secure: true });

        res.status(200).json(response);
      }
    } catch (err) {
      res.status(500).json({ err });
    }
  } else {
    response.resultado.statusCode = "404";
    response.resultado.statusText = "User Not Found";

    res.status(200).json(response);
  }
});

const generateJWT = (userId) => {
  const jwtBearerToken = jwt.sign({}, RSA_PRIVATE_KEY, {
    algorithm: "RS256",
    expiresIn: 2000,
    subject: userId,
  });

  return jwtBearerToken;
};
