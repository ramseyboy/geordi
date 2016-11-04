(defproject geordi "0.1.0-SNAPSHOT"
  :description "Java j-unit test generator"
  :url "https://github.com/ramseyboy/"
  :license {:name "Apache License, Version 2.0"
            :url "http://www.apache.org/licenses/LICENSE-2.0.html"}
  :dependencies [[org.clojure/clojure "1.8.0"]
                 [com.squareup/javapoet "1.7.0"]
                 [org.clojure/data.json "0.2.6"]]
  :main ^:skip-aot geordi.core
  :target-path "target/%s"
  :profiles {:uberjar {:aot :all}})
