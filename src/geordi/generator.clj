(ns geordi.generator)

;  def generateTests(String reqFileName) {
;    def parser = new SpecificationParser()
;    def spec = parser.parse(reqFileName)
;
;       def features = spec.features;
;
;       List<MethodSpec> methods = features.collect { feature ->
;        return MethodSpec.methodBuilder(feature.feature.toLowerCase().replaceAll(" ", "_").replaceAll(",", ""))
;                 .addModifiers(Modifier.PUBLIC)
;                 .returns(void.class)
;                 .addAnnotation(Test.class)
;                 .addStatement("\$T.assertTrue(\$L)", Assert.class, true)
;                 .addException(Exception.class)
;                 .build();
;        }
;
;    TypeSpec helloWorld = TypeSpec.classBuilder("${spec.specification}Test")
;    .addModifiers(Modifier.PUBLIC, Modifier.FINAL)
;    .addMethods(methods)
;    .build();
;
;    def javaFile = JavaFile.builder("me.ramseyboy.geordi", helloWorld)
;    .build();
;
;    javaFile.writeTo(new File("src/test/java/"))
;  }

(defn generate-tests
  ""
  [spec]
  (let [features (get spec :features)]
    (println (map #( %) features))))

