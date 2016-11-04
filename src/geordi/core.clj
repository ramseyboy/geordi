(ns geordi.core
  (:gen-class)
  (:require [clojure.java.io :as io]
            [geordi.parser :as parser]
            [geordi.generator :as gen]))

(defn -main
  "Generate j-unit tests based on file or directory"
  [& args]
  (let [req (-> "bdd_parser_test.json" io/resource io/file)]
    (let [spec (parser/parse (slurp req))]
      (gen/generate-tests spec))))
