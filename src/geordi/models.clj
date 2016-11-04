(ns geordi.models)

(defrecord Spec [spec, features])

(defrecord Feature [feature, block])

(defrecord Block [given, when, then])
