import React, { Component } from "react";
import { StyleSheet, View } from "react-native";
import { Text, Button, Form, Label, Input, Item, List, ListItem } from "native-base";
import { fonts, colors, padding } from "../../styles/common";

export default WalletDetailsSection = props => (
  <View style={styles.container}>
    <View style={styles.headerContainer}>
      <Text style={styles.headerTitle}>{props.investor.firstName} {props.investor.lastName}</Text>
      <Text style={styles.headerParagraph}>Current Amount: {props.investor.amount} $</Text>
    </View>
    <List>
        <ListItem itemHeader first>
            <Text>Wallet stocks</Text>
        </ListItem>
    </List>
  </View>
);

const styles = StyleSheet.create({
  container: {
    alignItems: "center",
    flex: 1,
    flexDirection: "column"
  },
  headerContainer: {
    alignSelf: "center",
    fontSize: fonts.lg,
    justifyContent: "center"
  },
  headerTitle: {
    fontWeight: "bold",
    color: colors.darkPrimary
  },
  headerParagraph: {
    fontSize: fonts.md,
    color: colors.darkSecondary
  },
  paragraph: {
    fontSize: fonts.md
  },
  textContainer: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center"
  },
  button: {
    width: "100%",
    alignSelf: "stretch",
    justifyContent: "center",
    paddingVertical: -padding.sm,
    marginTop: padding.md
  }
});
