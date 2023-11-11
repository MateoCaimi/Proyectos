var createError = require("http-errors");
var express = require("express");
var path = require("path");
var cookieParser = require("cookie-parser");
var logger = require("morgan");
var cors = require("cors");
var jwt = require("jsonwebtoken");
var fs = require("fs");

var indexRouter = require("./routes/index");
var loginRouter = require("./routes/login");
var maquinaRouter = require("./routes/maquina");
var tipoRouter = require("./routes/tipo");
var uploadRouter = require("./routes/upload");
var multimediaRouter = require("./routes/multimedia");

var app = express();
const RSA_PRIVATE_KEY = fs.readFileSync("private.key");

// view engine setup
app.set("views", path.join(__dirname, "views"));
app.set("view engine", "pug");

app.use(logger("dev"));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, "public")));
app.use(cors());

app.use((req, res, next) => {
  res.header("Access-Control-Allow-Origin", "*");
  res.header(
    "Access-Control-Allow-Headers",
    "Origin, X-Requested-With, Content-Type, Accept, Authorization"
  );

  if (req.method === "OPTIONS") {
    res.header("Access-Control-Allow-Methods", "PUT, POST, PATCH, DELETE, GET");
    return res.status(200).json({});
  }

  next();
});

app.use("/", indexRouter);
app.use("/maquinas", maquinaRouter);
app.use("/tipos", tipoRouter);
app.use("/multimedia", multimediaRouter);
app.use("/login", loginRouter);

app.use((req, res, next) => {
  const authHeader = req.headers.authorization;
  if (authHeader) {
    const token = authHeader.split(" ")[1];
    jwt.verify(token, RSA_PRIVATE_KEY, { algorithms: "RS256" }, (err) => {
      if (err) {
        const response = {
          Result: {
            statuscode: "403",
            statustext: "Unauthorized",
          },
          data: {},
        };

        res.status(403).json(response);
      } else {
        next();
      }
    });
  } else {
    const response = {
      Result: {
        statuscode: "401",
        statustext: "Unauthorized",
      },
      data: {},
    };
    res.status(401).json(response);
  }
});

app.use("/upload", uploadRouter);

// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get("env") === "development" ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render("error");
});

module.exports = app;
